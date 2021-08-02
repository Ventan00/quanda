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

            builder.HasKey(n => n.IdUser);

            builder.Property(n => n.IsSeen).IsRequired().HasDefaultValue(false);

            builder.Property(n => n.IdEntity).IsRequired(false);

            builder.HasOne(n => n.IdUserNavigation)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.IdUser)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Notification_User");

        }
    }
}
