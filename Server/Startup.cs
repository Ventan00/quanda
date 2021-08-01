using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Quanda.Server.Data;
using Quanda.Server.Extensions;
using Quanda.Server.Models.Settings;
using Quanda.Server.Repositories.Implementations;
using Quanda.Server.Repositories.Interfaces;
using Quanda.Server.Services.Implementations;
using Quanda.Server.Services.Interfaces;

namespace Quanda.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("MySqlDbConnString"),
                    new MySqlServerVersion(new Version(8, 0, 25)));
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(jwtBearerOptions =>
            {
                var jwtSettings = Configuration.GetSection("JwtSettings");
                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidateAudience = true,
                    ValidAudience = jwtSettings["Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]))
                };

                jwtBearerOptions.Events = new JwtBearerEvents
                {
                    //Zdarzenie wykonywane po poprawnej validacji tokenu.
                    //Dodaje id u¿ytkownika do rz¹dania http
                    OnTokenValidated = tokenValidatedContext =>
                    {
                        try
                        {
                            tokenValidatedContext.HttpContext.Request
                                .SetUserId(tokenValidatedContext.Principal);
                        }
                        catch
                        {
                            tokenValidatedContext.Response.StatusCode = 401;
                            tokenValidatedContext.Response.CompleteAsync();
                        }

                        return Task.CompletedTask;
                    }
                };
            });

            services.AddControllersWithViews();
            services.AddRazorPages();

            //repositories
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IAnswerRepository, AnswerRepository>();
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<ITempUsersRepository, TempUsersRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();

            //custom services
            services.AddScoped<IUserAuthService, UserAuthService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<ISmtpService, SmtpService>();
            services.AddHttpContextAccessor();
            services.AddScoped<ICaptchaService, CaptchaService>();

            //ConfigurationModels
            services.Configure<JwtConfigModel>(Configuration.GetSection("JwtSettings"));
            services.Configure<SmtpConfigModel>(Configuration.GetSection("SmtpSettings"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}