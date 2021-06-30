using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quanda.Shared.Models;

namespace Quanda.Server.EfConfigurations
{
    public class TempUserEfConfiguration : IEntityTypeConfiguration<TempUser>
    {
        public void Configure(EntityTypeBuilder<TempUser> builder)
        {
            builder.ToTable("Temp_User");

            builder.HasKey(tu => tu.IdUser)
                .HasName("TempUser_pk");

            builder.Property(tu => tu.Code).HasMaxLength(36).IsRequired();

            builder.Property(tu => tu.ExpirationDate).IsRequired();

            builder.HasOne(tu => tu.IdUserNavigation)
                .WithOne(u => u.IdTempUserNavigation)
                .HasForeignKey<TempUser>(tu => tu.IdUser)
                .HasConstraintName("TempUser_User");
        }
    }
}
