using Application.Interfaces;
using MediatR;

namespace Application.RepetitionGroups.UseCases.Commands
{
    public class CompleteRepetitionCommandHandler : IRequestHandler<CompleteRepetitionCommand, bool>
    {
        private readonly IRepetitionGroupRepository _repetitionGroupRepository;
        public CompleteRepetitionCommandHandler(IRepetitionGroupRepository repetitionGroupRepository)
        {
            _repetitionGroupRepository = repetitionGroupRepository;
        }
        public async Task<bool> Handle(CompleteRepetitionCommand request, CancellationToken cancellationToken)
        {
            return await _repetitionGroupRepository.CompleteRepetitionAsync(request.Id);
        }
    }
}
