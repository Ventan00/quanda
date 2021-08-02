using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quanda.Shared.Models;

namespace Quanda.Server.EfConfigurations
{
    public class QuestionTagEfConfiguration : IEntityTypeConfiguration<QuestionTag>
    {
        public void Configure(EntityTypeBuilder<QuestionTag> builder)
        {
            builder.ToTable("Question_Tag");

            builder.HasKey(qc => new { qc.IdQuestion, qc.IdTag });

            builder.HasOne(qc => qc.IdTagNavigation)
                .WithMany(c => c.QuestionTags)
                .HasForeignKey(qc => qc.IdTag)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("QuestionTag_Tag");

            builder.HasOne(qc => qc.IdQuestionNavigation)
                .WithMany(c => c.QuestionTags)
                .HasForeignKey(qc => qc.IdQuestion)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("QuestionTag_Question");
        }
    }
}
