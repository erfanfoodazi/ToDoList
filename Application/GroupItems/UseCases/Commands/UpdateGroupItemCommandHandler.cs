using Application.Interfaces;
using Domain.Enums;
using MediatR;

namespace Application.GroupItems.UseCases.Commands
{
    public class UpdateGroupItemCommandHandler : IRequestHandler<UpdateGroupItemCommand, bool>
    {
        private readonly IGroupItemRepository _groupItemRepository;
        public UpdateGroupItemCommandHandler(IGroupItemRepository groupItemRepository)
        {
            _groupItemRepository = groupItemRepository;
        }
        public async Task<bool> Handle(UpdateGroupItemCommand request, CancellationToken cancellationToken)
        {
            var existGroup = await _groupItemRepository.GetGroupItemByIdAsync(request.Id);

            if (existGroup == null)
                return false;

            existGroup.Title = request.Title;
            existGroup.RepetitionType = (RepetitionType)request.RepetitionType;
            existGroup.EndRepit = request.EndDate;
            
            return await _groupItemRepository.UpdateGroupItemAsync(existGroup);
        }
    }
}
