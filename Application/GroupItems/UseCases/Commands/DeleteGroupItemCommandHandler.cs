using Application.Interfaces;
using MediatR;

namespace Application.GroupItems.UseCases.Commands
{
    public class DeleteGroupItemCommandHandler : IRequestHandler<DeleteGroupItemCommand, bool>
    {
        private readonly IGroupItemRepository _groupItemRepository;
        public DeleteGroupItemCommandHandler(IGroupItemRepository groupItemRepository)
        {
            _groupItemRepository = groupItemRepository;
        }
        public async Task<bool> Handle(DeleteGroupItemCommand request, CancellationToken cancellationToken)
        {
            return await _groupItemRepository.DeleteGroupItemAsync(request.Id);
        }
    }
}
