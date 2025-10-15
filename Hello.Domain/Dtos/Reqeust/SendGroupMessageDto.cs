using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hello.Application.Dtos.Reqeust
{
    public class SendGroupMessageDto
    {
        public int GroupId { get; set; }
        public string? Message { get; set; }
        public string? ImageBase64 { get; set; }
    }
}
