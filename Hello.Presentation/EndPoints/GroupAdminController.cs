using Hello.Application.Features.Groups.Command.AddAdmin;
using Hello.Application.Features.Groups.Command.Delete;
using Hello.Application.Features.Groups.Command.DeleteAdmin;
using Hello.Application.Features.Groups.Command.Update;
using Hello.Domain.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hello.Presentation.EndPoints
{
    public class GroupAdminController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public GroupAdminController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpDelete("group/{groupId}")]
        public async Task<ActionResult<Result<int>>> DeleteGroup(int groupId)
        {
            return await HandleResponse(await _mediator.Send(new DeleteGroupCommand(groupId)));
        }

        [HttpDelete("group/admin")]
        public async Task<ActionResult<Result<bool>>> DeleteGroupAdmin([FromBody] DeleteGroupAdminCommand command)
        {
            return await HandleResponse(await _mediator.Send(command));
        }

        [HttpPut("group")]
        public async Task<ActionResult<Result<int>>> UpdateGroup(UpdateGroupCommand command)
        {
            return await HandleResponse(await _mediator.Send(command));
        }

        [HttpPost("group/addAdmin")]
        public async Task<ActionResult> AddAdminToGroup(AddAdminCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
