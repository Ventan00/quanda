using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quanda.Shared.Models;

namespace Quanda.Server.EfConfigurations
{
    public class TagEfConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.ToTable("Tag");

            builder.HasKey(t => t.IdTag)
                .HasName("Tag_pk");

            builder.Property(t => t.IdTag)
                .UseMySqlIdentityColumn();

            builder.Property(t => t.Name).HasMaxLength(30).IsRequired();

            builder.Property(t => t.IdMainTag).IsRequired(false);

            builder.Property(t => t.Description).IsRequired();

            builder.HasOne(t => t.IdMainTagNavigation)
                .WithMany(t => t.InverseIdMainTagsNavigation)
                .HasForeignKey(t => t.IdMainTag)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Tag_Tag");
        }
    }
}
