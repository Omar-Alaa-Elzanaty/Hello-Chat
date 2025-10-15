using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hello.Application.Features.Chat.Queries.GetUnReadMessage
{
    public class GetUnReadMessageQueryDto
    {
        public List<LastGroupChatDto> GroupsChats { get; set; }
        public List<LastChatDto> Chats { get; set; }
    }
    public class LastChatDto
    {
        public string Id { get; set; }
        public string Sender { get; set; }
        public string? Message { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public int TotalUnReadCount { get; set; }
    }

    public class LastGroupChatDto
    {
        public int Id { get; set; }
        public string Sender { get; set; }
        public string? Message { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public int TotalUnreadCount { get; set; }
    }
}
