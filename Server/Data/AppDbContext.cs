using Microsoft.EntityFrameworkCore;
using Quanda.Shared.Models;

namespace Quanda.Server.Data
{
    public class AppDbContext : DbContext
    {
        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<QuestionCategory> QuestionCategories { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
