using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.RepetitionGroups.UseCases.Queries
{
    public class GetRepitedGroupByIdQueryHandler : IRequestHandler<GetRepitedGroupByIdQuery, RepetedGroup?>
    {
        private readonly IRepetitionGroupRepository _repetitionGroupRepository;
        public GetRepitedGroupByIdQueryHandler(IRepetitionGroupRepository repetitionGroupRepository)
        {
            _repetitionGroupRepository = repetitionGroupRepository;
        }
        public async Task<RepetedGroup?> Handle(GetRepitedGroupByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repetitionGroupRepository.GetRepetitionGroupByIdAsync(request.Id);
        }
    }
}
