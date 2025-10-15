using Hello.Application.Dtos.Response;
using Hello.Domain.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hello.Domain.Interfaces
{
    public interface IChatHub
    {
        Task ReceiveGroupMessage(ReceiveGroupMessageDto dto);
        Task ReceivePrivateMessage(ReceivePrivateMessageDto dto);
        Task ReceiveRemoveFromGroup(string message);
        Task UserTyping(string message);
        Task ReceiveGroupUpdate(string message);
        Task MarkMessageAsRead(int messageId);
    }
}
