using Hello.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hello.Persistence.EntitiesConfiguration
{
    internal class GroupChatReadConfig : IEntityTypeConfiguration<GroupChatRead>
    {
        public void Configure(EntityTypeBuilder<GroupChatRead> builder)
        {
            builder.HasKey(x => new { x.MemberId, x.GroupChatId });

            builder.HasOne(x => x.Member)
                .WithMany()
                .HasForeignKey(x => x.MemberId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
