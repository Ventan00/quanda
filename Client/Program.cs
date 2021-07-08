using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Blazored.Toast;
using Microsoft.AspNetCore.Components.Authorization;
using Quanda.Client.Authentication;
using Quanda.Client.Repositories.Implementations;
using Quanda.Client.Repositories.Interfaces;
using Quanda.Client.Helpers;

namespace Quanda.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            ConfigureServices(builder.Services);

            await builder.Build().RunAsync();
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddBlazoredLocalStorage();
            services.AddAuthorizationCore();
            services.AddBlazoredToast();

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IHttpService, HttpService>();
            services.AddScoped<IAnswerRepository, AnswerRepository>();
        }
    }
}
