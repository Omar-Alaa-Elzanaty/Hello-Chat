using Hello.Application.Features.Chat.Queries.GetUnReadMessage;
using Hello.Application.Features.Groups.Command.AddAdmin;
using Hello.Application.Features.Groups.Command.Create;
using Hello.Application.Features.Groups.Command.Delete;
using Hello.Application.Features.Groups.Command.Update;
using Hello.Domain.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hello.Presentation.EndPoints
{
    [Authorize]
    public class ChatController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public ChatController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("group")]
        public async Task<ActionResult<Result<int>>> CreateGroup(CreateGroupCommand command)
        {
            return await HandleResponse(await _mediator.Send(command));
        }

        [HttpGet("lastMessages")]
        public async Task<ActionResult<Result<GetUnReadMessageQueryDto>>> GetUnReadMessages()
        {
            return await HandleResponse(await _mediator.Send(new GetUnReadMessageQuery()));
        }
    }
}
