using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quanda.Shared.Models;

namespace Quanda.Server.EfConfigurations
{
    public class ReportEfConfiguration : IEntityTypeConfiguration<Report>
    {
        public void Configure(EntityTypeBuilder<Report> builder)
        {
            builder.ToTable("Report");

            builder.HasKey(r => r.IdReport).HasName("Report_pk");

            builder.Property(r => r.IdReport).UseMySqlIdentityColumn();

            builder.Property(r => r.IdMessage).IsRequired(false);

            builder.Property(r => r.IdAnswer).IsRequired(false);

            builder.Property(r => r.IdQuestion).IsRequired(false);

            builder.HasOne(r => r.IdMessageNavigation)
                .WithMany(m => m.Reports)
                .HasForeignKey(r => r.IdMessage)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Report_Message");

            builder.HasOne(r => r.IdIssuerNavigation)
                .WithMany(u => u.Reports)
                .HasForeignKey(r => r.IdIssuer)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Report_User");

            builder.HasOne(r => r.IdAnswerNavigation)
                .WithMany(a => a.Reports)
                .HasForeignKey(r => r.IdAnswer)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Report_Answer");

            builder.HasOne(r => r.IdQuestionNavigation)
                .WithMany(q => q.Reports)
                .HasForeignKey(r => r.IdQuestion)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Report_Question");

        }
    }
}
