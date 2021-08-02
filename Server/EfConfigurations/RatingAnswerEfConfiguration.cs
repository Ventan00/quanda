using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quanda.Shared.Models;

namespace Quanda.Server.EfConfigurations
{
    public class RatingAnswerEfConfiguration : IEntityTypeConfiguration<RatingAnswer>
    {
        public void Configure(EntityTypeBuilder<RatingAnswer> builder)
        {
            builder.ToTable("Rating_Answer");

            builder.HasKey(ra => new { ra.IdAnswer, ra.IdUser }).HasName("RatingAnswer_pk");

            builder.Property(ra => ra.Value).IsRequired();

            builder.HasOne(ra => ra.IdAnswerNavigation)
                .WithMany(ra => ra.RatingAnswers)
                .HasForeignKey(ra => ra.IdAnswer)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("RatingAnswer_Answer");

            builder.HasOne(ra => ra.IdUserNavigation)
                .WithMany(ra => ra.RatingAnswers)
                .HasForeignKey(ra => ra.IdUser)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("RatingAnswer_User");
        }
    }
}
