using Hello.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hello.Persistence.EntitiesConfiguration
{
    internal class CommunityChatConfig : IEntityTypeConfiguration<CommunityChat>
    {
        public void Configure(EntityTypeBuilder<CommunityChat> builder)
        {
            builder.HasOne(x => x.Community)
                .WithMany(x => x.Chats)
                .HasForeignKey(x => x.CommunityId);

        }
    }
}
