using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quanda.Shared.Models;

namespace Quanda.Server.EfConfigurations
{
    public class TagUserEfConfiguration : IEntityTypeConfiguration<TagUser>
    {
        public void Configure(EntityTypeBuilder<TagUser> builder)
        {
            builder.ToTable("Tag_User");

            builder.HasKey(tu => new { tu.IdUser, tu.IdTag }).HasName("TagUser_pk");

            builder.HasOne(tu => tu.IdTagNavigation)
                .WithMany(t => t.TagUsers)
                .HasForeignKey(tu => tu.IdTag)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TagUser_Tag");

            builder.HasOne(tu => tu.IdUserNavigation)
                .WithMany(u => u.TagUsers)
                .HasForeignKey(tu => tu.IdUser)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("TagUser_User");
        }
    }
}
