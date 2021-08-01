using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quanda.Shared.Models;

namespace Quanda.Server.EfConfigurations
{
    public class ThreadEfConfiguration : IEntityTypeConfiguration<Thread>
    {
        public void Configure(EntityTypeBuilder<Thread> builder)
        {
            builder.ToTable("Thread");

            builder.HasKey(t => t.IdThread).HasName("Thread_pk");

            builder.Property(t => t.IdThread).UseMySqlIdentityColumn();

            builder.Property(t => t.Header).HasMaxLength(50).IsRequired();

            builder.HasOne(t => t.IdReceiverNavigation)
                .WithMany(u => u.ReceiverThreads)
                .HasForeignKey(t => t.IdReceiver)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Thread_Receiver");

            builder.HasOne(t => t.IdSenderNavigation)
                .WithMany(u => u.SenderThreads)
                .HasForeignKey(t => t.IdSender)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Thread_Sender");
        }
    }
}
