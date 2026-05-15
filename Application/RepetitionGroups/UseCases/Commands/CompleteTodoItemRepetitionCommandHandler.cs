using Application.Interfaces;
using MediatR;

namespace Application.RepetitionGroups.UseCases.Commands
{
    public class CompleteTodoItemRepetitionCommandHandler : IRequestHandler<CompleteTodoItemRepetitionCommand, bool>
    {
        private readonly IRepetitionGroupRepository _repetitionGroupRepository;
        public CompleteTodoItemRepetitionCommandHandler(IRepetitionGroupRepository repetitionGroupRepository)
        {
            _repetitionGroupRepository = repetitionGroupRepository;
        }
        public async Task<bool> Handle(CompleteTodoItemRepetitionCommand request, CancellationToken cancellationToken)
        {
            var result = await _repetitionGroupRepository.CompleteTodoItemRepetitionAsync(request.TodoId);

            return result;
        }
    }
}
