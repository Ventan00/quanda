using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quanda.Shared.Models;

namespace Quanda.Server.EfConfigurations
{
    public class MessageEfConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable("Message");

            builder.HasKey(m => m.IdMessage).HasName("Message_pk");

            builder.Property(m => m.IdMessage).UseMySqlIdentityColumn();

            builder.Property(m => m.Text).IsRequired();

            builder.HasOne(m => m.IdThreadNavigation)
                .WithMany(t => t.Messages)
                .HasForeignKey(m => m.IdThread)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Message_Thread");

            builder.HasOne(m => m.IdSenderNavigation)
                .WithMany(u => u.Messages)
                .HasForeignKey(m => m.IdSender)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Message_User");
        }
    }
}
