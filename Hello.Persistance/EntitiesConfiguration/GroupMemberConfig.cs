using Hello.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hello.Persistence.EntitiesConfiguration
{
    public class GroupMemberConfig : IEntityTypeConfiguration<GroupMember>
    {
        public void Configure(EntityTypeBuilder<GroupMember> builder)
        {
            builder.HasKey(x => new { x.MemberId, x.GroupId });

            builder.HasOne(x => x.Member)
                .WithMany(x => x.Groups)
                .HasForeignKey(x => x.MemberId);

            builder.HasOne(x => x.Group)
                .WithMany(x => x.Members)
                .HasForeignKey(x => x.GroupId);
        }
    }
}
