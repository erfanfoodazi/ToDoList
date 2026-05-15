// Application/RepetitionGroups/UseCases/Commands/AddTodoItemToRepetitionCommandHandler.cs
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.RepetitionGroups.UseCases.Commands
{
    public class AddTodoItemToRepetitionCommandHandler : IRequestHandler<AddTodoItemToRepetitionCommand, bool>
    {
        private readonly IRepetitionGroupRepository _repetitionRepository;

        public AddTodoItemToRepetitionCommandHandler(IRepetitionGroupRepository repetitionRepository)
        {
            _repetitionRepository = repetitionRepository;
        }

        public async Task<bool> Handle(AddTodoItemToRepetitionCommand request, CancellationToken cancellationToken)
        {
            var result = await _repetitionRepository.AddTodoItemToAllRepetitionsAsync(
                request.GroupId,
                request.TodoItem
            );
            return result;
        }
    }
}