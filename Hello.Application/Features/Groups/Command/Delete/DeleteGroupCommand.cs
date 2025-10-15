using Hello.Domain.Dtos;
using Hello.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hello.Application.Features.Groups.Command.Delete
{
    public class DeleteGroupCommand : IRequest<Result<int>>
    {
        public int GroupId { get; set; }

        public DeleteGroupCommand(int groupId)
        {
            GroupId = groupId;
        }
    }

    internal class DeleteGroupCommandHandler : IRequestHandler<DeleteGroupCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteGroupCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(DeleteGroupCommand request, CancellationToken cancellationToken)
        {
            var isGroupFound = await _unitOfWork.GroupRepo.Entities
                .AnyAsync(x => x.Id == request.GroupId, cancellationToken);

            if (!isGroupFound)
            {
                return Result<int>.Failure("Group not found.");
            }

            await _unitOfWork.GroupRepo.Entities
                .Where(x => x.Id == request.GroupId)
                .ExecuteDeleteAsync(cancellationToken);

            return Result<int>.Success();
        }
    }
}
