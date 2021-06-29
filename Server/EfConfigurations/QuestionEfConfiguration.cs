using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quanda.Shared.Models;

namespace Quanda.Server.EfConfigurations
{
    public class QuestionEfConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.ToTable("Question");

            builder.HasKey(q => q.IdQuestion)
                .HasName("Question_pk");

            builder.Property(q => q.IdQuestion)
                .UseMySqlIdentityColumn();

            builder.Property(q => q.Header).HasMaxLength(50).IsRequired();

            builder.Property(q => q.Description).IsRequired().HasColumnType("text");

            builder.Property(q => q.PublishDate).IsRequired();

            builder.Property(q => q.Views).IsRequired().HasDefaultValue(0);

            builder.Property(q => q.IdUser).IsRequired();

            builder.Property(q => q.IsFinished).IsRequired().HasDefaultValue(false);

            builder.Property(q => q.ToCheck).IsRequired().HasDefaultValue(false);

            builder.Property(q => q.IsModified).IsRequired().HasDefaultValue(false);
        }
    }
}
