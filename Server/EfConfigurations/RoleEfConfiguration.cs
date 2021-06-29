using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quanda.Shared.Models;

namespace Quanda.Server.EfConfigurations
{
    public class RoleEfConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Role");

            builder.HasKey(r => r.IdRole)
                .HasName("Role_pk");

            builder.Property(r => r.IdRole)
                .UseMySqlIdentityColumn();

            builder.Property(r => r.Name).HasMaxLength(30).IsRequired();
        }
    }
}
