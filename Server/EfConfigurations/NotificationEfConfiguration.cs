using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quanda.Shared.Models;

namespace Quanda.Server.EfConfigurations
{
    public class NotificationEfConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notification");

            builder.HasKey(n => new { n.IdQuestion, n.IdUser });

            builder.HasOne(n => n.IdUserNavigation)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("User_Notification");

            builder.HasOne(n => n.IdQuestionNavigation)
                .WithMany(q => q.Notifications)
                .HasForeignKey(n => n.IdQuestion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Question_Notification");
        }
    }
}
