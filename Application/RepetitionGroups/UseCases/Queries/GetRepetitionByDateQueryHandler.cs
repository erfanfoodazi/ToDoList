using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.RepetitionGroups.UseCases.Queries
{
    public class GetRepetitionByDateQueryHandler : IRequestHandler<GetRepetitionByDateQuery, List<RepetedGroup>>
    {
        private readonly IRepetitionGroupRepository _repetitionGroupRepository;
        public GetRepetitionByDateQueryHandler(IRepetitionGroupRepository repetitionGroupRepository)
        {
            _repetitionGroupRepository = repetitionGroupRepository;
        }
        public async Task<List<RepetedGroup>> Handle(GetRepetitionByDateQuery request, CancellationToken cancellationToken)
        {
            return await _repetitionGroupRepository.GetPendingRepetitionsByDateAsync(request.Date);
        }
    }
}
