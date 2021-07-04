using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quanda.Shared.Models;

namespace Quanda.Server.EfConfigurations
{
    public class UserEfConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");

            builder.HasKey(u => u.IdUser)
                .HasName("User_pk");

            builder.Property(u => u.IdUser)
                .UseMySqlIdentityColumn();

            builder.Property(u => u.Nickname).HasMaxLength(30).IsRequired();

            builder.HasIndex(u => u.Nickname).IsUnique().HasDatabaseName("Unique_nickname");

            builder.Property(u => u.FirstName).HasMaxLength(30).IsRequired(false);

            builder.Property(u => u.LastName).HasMaxLength(30).IsRequired(false);

            builder.Property(u => u.Email).IsRequired();

            builder.HasIndex(u => u.Email).IsUnique().HasDatabaseName("Unique_email");

            builder.Property(u => u.HashedPassword).HasMaxLength(84).IsRequired();

            builder.Property(u => u.Points).IsRequired().HasDefaultValue(0);

            builder.Property(u => u.PhoneNumber).HasMaxLength(9).IsRequired(false);

            builder.Property(u => u.Bio).IsRequired(false);

            builder.Property(u => u.Avatar).IsRequired(false);

            builder.Property(u => u.RegistrationDate).IsRequired();

            builder.Property(u => u.RefreshToken).HasMaxLength(36).IsRequired(false);

            builder.Property(u => u.RefreshTokenExpirationDate).IsRequired(false);

            builder.Property(u => u.IdService).IsRequired(false);

            builder.Property(u => u.ServiceToken).IsRequired(false);

            builder.HasMany(u => u.Questions)
                .WithOne(q => q.IdUserNavigation)
                .HasForeignKey(q => q.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("User_Question");
        }
    }
}
