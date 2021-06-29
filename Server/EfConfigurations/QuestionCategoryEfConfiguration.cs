using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quanda.Shared.Models;

namespace Quanda.Server.EfConfigurations
{
    public class QuestionCategoryEfConfiguration : IEntityTypeConfiguration<QuestionCategory>
    {
        public void Configure(EntityTypeBuilder<QuestionCategory> builder)
        {
            builder.ToTable("Question_Category");

            builder.HasKey(qc => new { qc.IdQuestion, qc.IdCategory });

            builder.HasOne(qc => qc.IdCategoryNavigation)
                .WithMany(c => c.QuestionCategories)
                .HasForeignKey(qc => qc.IdCategory)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Category_QuestionCategory");

            builder.HasOne(qc => qc.IdQuestionNavigation)
                .WithMany(c => c.QuestionCategories)
                .HasForeignKey(qc => qc.IdQuestion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Question_QuestionCategory");
        }
    }
}
