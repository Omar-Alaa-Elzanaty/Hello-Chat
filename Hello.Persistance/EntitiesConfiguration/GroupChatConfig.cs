using Hello.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hello.Persistence.EntitiesConfiguration
{
    internal class GroupChatConfig : IEntityTypeConfiguration<GroupChat>
    {
        public void Configure(EntityTypeBuilder<GroupChat> builder)
        {
            builder.HasOne(x => x.Sender)
                .WithMany()
                .HasForeignKey(x => x.SenderId);

            builder.HasOne(x => x.Group)
                .WithMany(x => x.Chats)
                .HasForeignKey(x => x.GroupId);
        }
    }
}
