using FluentValidation;
using Hello.Domain.Dtos;
using Hello.Domain.Interfaces;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Hello.Application.Features.Groups.Command.Update
{
    public class UpdateGroupCommand:IRequest<Result<int>>
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? ImageBase64 { get; set; }
    }

    internal class UpdateGroupCommandHandler : IRequestHandler<UpdateGroupCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _context;
        private readonly IValidator<UpdateGroupCommand> _validator;

        public UpdateGroupCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<UpdateGroupCommand> validator,
            IHttpContextAccessor context)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validator = validator;
            _context = context;
        }
        public async Task<Result<int>> Handle(UpdateGroupCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if(!validationResult.IsValid)
            {
                return Result<int>.ValidationFailure(validationResult.Errors);
            }

            var userId = _context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            if(!await _unitOfWork.GroupMemberRepo.Entities
                .AnyAsync(x=>x.MemberId==userId && x.IsAdmin, cancellationToken))
            {
                return Result<int>.Failure("user not allowed.", HttpStatusCode.MethodNotAllowed);
            }

            var group = await _unitOfWork.GroupRepo.Entities
                .FirstOrDefaultAsync(x => x.Id == request.id, cancellationToken);

            if(group is null)
            {
                return Result<int>.Failure("Group not found.", HttpStatusCode.NotFound);
            }

            _mapper.Map(request, group);

            await _unitOfWork.GroupRepo.AddAsync(group);
            await _unitOfWork.SaveChangesAsync();

            return Result<int>.Success();
        }
    }
}
