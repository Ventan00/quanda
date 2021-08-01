using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quanda.Shared.Models;

namespace Quanda.Server.EfConfigurations
{
    public class RecoveryUserEfConfiguration : IEntityTypeConfiguration<RecoveryUser>
    {
        public void Configure(EntityTypeBuilder<RecoveryUser> builder)
        {
            builder.ToTable("Recovery_User");

            builder.HasKey(ru => ru.IdUser)
                .HasName("RecoveryUser_pk");

            builder.Property(ru => ru.Code).HasMaxLength(36).IsRequired();

            builder.HasIndex(ru => ru.Code).IsUnique().HasDatabaseName("Unique_code");

            builder.Property(ru => ru.ExpirationDate).IsRequired();

            builder.HasOne(ru => ru.IdUserNavigation)
                .WithOne(u => u.IdRecoveryUserNavigation)
                .HasForeignKey<RecoveryUser>(ru => ru.IdUser)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("RecoveryUser_User");
        }
    }
}
