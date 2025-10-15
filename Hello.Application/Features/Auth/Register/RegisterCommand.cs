using FluentValidation;
using Hello.Domain.Dtos;
using Hello.Domain.Interfaces;
using Hello.Domain.Models;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hello.Application.Features.Auth.Register
{
    public class RegisterCommand : IRequest<Result<string>>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    internal class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<RegisterCommand> _validator;
        private readonly UserManager<User> _userManager;

        public RegisterCommandHandler(
            IUnitOfWork unitOfWork,
            IValidator<RegisterCommand> validator,
            UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
            _userManager = userManager;
        }

        public async Task<Result<string>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                return Result<string>.ValidationFailure(validationResult.Errors);
            }

            if (await _unitOfWork.UserNameBloomServices.MightContainUserNameAsync(request.UserName)
                && await _userManager.Users.AnyAsync(x => x.NormalizedUserName == request.UserName.Normalize(), cancellationToken))
            {
                return Result<string>.Failure("username already taken.");
            }

            var user = request.Adapt<User>();

            var identityResult = await _userManager.CreateAsync(user, request.Password);

            if (!identityResult.Succeeded)
            {
                return Result<string>.ValidationFailure(identityResult.Errors);
            }

            await _unitOfWork.UserNameBloomServices.AddUserNameAsync(request.UserName);

            return Result<string>.Success(data: user.Id);
        }
    }
}
