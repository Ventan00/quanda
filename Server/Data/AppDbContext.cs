using Microsoft.EntityFrameworkCore;
using Quanda.Server.EfConfigurations;
using Quanda.Shared.Models;

namespace Quanda.Server.Data
{
    public class AppDbContext : DbContext
    {
        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<QuestionTag> QuestionTags { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<TempUser> TempUsers { get; set; }
        public virtual DbSet<RatingAnswer> RatingAnswers { get; set; }
        public virtual DbSet<Thread> Threads { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
        public virtual DbSet<TagUser> TagUsers { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AnswerEfConfiguration());
            modelBuilder.ApplyConfiguration(new TagEfConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationEfConfiguration());
            modelBuilder.ApplyConfiguration(new QuestionEfConfiguration());
            modelBuilder.ApplyConfiguration(new QuestionTagEfConfiguration());
            modelBuilder.ApplyConfiguration(new RoleEfConfiguration());
            modelBuilder.ApplyConfiguration(new ServiceEfConfiguration());
            modelBuilder.ApplyConfiguration(new UserEfConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleEfConfiguration());
            modelBuilder.ApplyConfiguration(new TempUserEfConfiguration());
            modelBuilder.ApplyConfiguration(new RatingAnswerEfConfiguration());
            modelBuilder.ApplyConfiguration(new ThreadEfConfiguration());
            modelBuilder.ApplyConfiguration(new MessageEfConfiguration());
            modelBuilder.ApplyConfiguration(new ReportEfConfiguration());
            modelBuilder.ApplyConfiguration(new TagUserEfConfiguration());
        }
    }
}
