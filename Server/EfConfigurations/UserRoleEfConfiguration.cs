using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quanda.Shared.Models;

namespace Quanda.Server.EfConfigurations
{
    public class UserRoleEfConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("User_Role");

            builder.HasKey(ur => new { ur.IdRole, ur.IdUser })
                .HasName("UserRole_pk");

            builder.Property(ur => ur.ExpirationDate).IsRequired(false);

            builder.HasOne(ur => ur.IdRoleNavigation)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.IdRole)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Role_UserRole");


            builder.HasOne(ur => ur.IdUserNavigation)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.IdUser)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("User_UserRole");
        }
    }
}
