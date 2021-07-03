using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quanda.Shared.Models;

namespace Quanda.Server.EfConfigurations
{
    public class AnswerEfConfiguration : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> builder)
        {
            builder.ToTable("Answer");

            builder.HasKey(a => a.IdAnswer)
                .HasName("Answer_pk");

            builder.Property(a => a.IdAnswer)
                .UseMySqlIdentityColumn();

            builder.Property(a => a.Text).IsRequired().HasColumnType("text");

            builder.Property(a => a.IsModified).IsRequired().HasDefaultValue(false);

            builder.Property(a => a.IdQuestion).IsRequired();

            builder.Property(a => a.IdUser).IsRequired();

            builder.Property(a => a.IdRootAnswer).IsRequired(false);

            builder.HasOne(a => a.IdQuestionNavigation)
                .WithMany(q => q.Answers)
                .HasForeignKey(a => a.IdQuestion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Answer_Question");


            builder.HasOne(a => a.IdUserNavigation)
                .WithMany(u => u.Answers)
                .HasForeignKey(a => a.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Answer_User");

            builder.HasOne(a => a.IdRootAnswerNavigation)
                .WithMany(a => a.InverseIdRootAnswersNavigation)
                .HasForeignKey(a => a.IdRootAnswer)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Answer_Answer");
        }
    }
}
