using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quanda.Shared.Models;

namespace Quanda.Server.EfConfigurations
{
    public class ServiceEfConfiguration : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> builder)
        {
            builder.ToTable("Service");

            builder.HasKey(s => s.IdService)
                .HasName("Service_pk");

            builder.Property(s => s.IdService)
                .UseMySqlIdentityColumn();

            builder.Property(s => s.Name).HasMaxLength(50).IsRequired();

            builder.Property(s => s.Connection).IsRequired();

            builder.HasMany(s => s.Users)
                .WithOne(u => u.IdServiceNavigation)
                .HasForeignKey(u => u.IdService)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Service_User");
        }
    }
}
