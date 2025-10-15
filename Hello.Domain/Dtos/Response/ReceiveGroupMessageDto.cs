using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hello.Application.Dtos.Response
{
    public class ReceiveGroupMessageDto
    {
        public string UserId { get; set; }
        public int MessageId { get; set; }
        public string Name { get; set; }
        public string? Message { get; set; }
        public string? ImageUrl { get; set; }
    }
}
