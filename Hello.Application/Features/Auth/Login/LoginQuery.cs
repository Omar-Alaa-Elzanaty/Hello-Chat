using Hello.Domain.Dtos;
using Hello.Domain.Interfaces;
using Hello.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Hello.Application.Features.Auth.Login
{
    public class LoginQuery : IRequest<Result<LoginQueryDto>>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    internal class LoginQueryHandler : IRequestHandler<LoginQuery, Result<LoginQueryDto>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IAuthSerivces _authServices;
        public LoginQueryHandler(
            UserManager<User> userManager,
            IAuthSerivces authServices)
        {
            _userManager = userManager;
            _authServices = authServices;
        }

        public async Task<Result<LoginQueryDto>> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);

            if (user == null)
            {
                return Result<LoginQueryDto>.Failure("Invalid username or password");
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!isPasswordValid)
            {
                return Result<LoginQueryDto>.Failure("Invalid username or password");
            }

            var response = new LoginQueryDto
            {
                Id = user.Id,
                Name = user.FirstName + ' ' + user.LastName,
                Token = await _authServices.GenerateTokenAsync(user)
            };

            return Result<LoginQueryDto>.Success(response);
        }
    }
}
