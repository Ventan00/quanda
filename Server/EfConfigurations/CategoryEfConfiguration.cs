using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quanda.Shared.Models;

namespace Quanda.Server.EfConfigurations
{
    public class CategoryEfConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Category");

            builder.HasKey(c => c.IdCategory)
                .HasName("Category_pk");

            builder.Property(c => c.IdCategory)
                .UseMySqlIdentityColumn();

            builder.Property(c => c.Name).HasMaxLength(30).IsRequired();

            builder.Property(c => c.IdMainCategory).IsRequired(false);

            builder.HasOne(c => c.IdMainCategoryNavigation)
                .WithMany(c => c.InverseIdMainCategoriesNavigation)
                .HasForeignKey(c => c.IdMainCategory)
                .HasConstraintName("Category_Category");
        }
    }
}
