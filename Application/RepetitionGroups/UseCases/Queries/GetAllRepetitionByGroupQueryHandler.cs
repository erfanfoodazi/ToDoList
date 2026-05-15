using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.RepetitionGroups.UseCases.Queries
{
    public class GetAllRepetitionByGroupQueryHandler : IRequestHandler<GetAllRepetitionByGroupQuery, List<RepetedGroup>>
    {
        private readonly IRepetitionGroupRepository _repetitionGroupRepository;
        public GetAllRepetitionByGroupQueryHandler(IRepetitionGroupRepository repetitionGroupRepository)
        {
            _repetitionGroupRepository = repetitionGroupRepository;
        }
        public async Task<List<RepetedGroup>> Handle(GetAllRepetitionByGroupQuery request, CancellationToken cancellationToken)
        {
            return await _repetitionGroupRepository.GetAllRepetationsByGroupIdAsync(request.GroupId);
        }
    }
}
