using Hello.Application.Features.Auth.Login;
using Hello.Application.Features.Auth.Register;
using Hello.Domain.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hello.Presentation.EndPoints
{
    public class AuthController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<ActionResult<Result<string>>> Register(RegisterCommand command)
        {
            var result = await _mediator.Send(command);
            return await HandleResponse(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<Result<LoginQueryDto>>> Login(LoginQuery query)
        {
            var result = await _mediator.Send(query);
            return await HandleResponse(result);
        }
    }
}
