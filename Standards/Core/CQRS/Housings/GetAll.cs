using MediatR;
using Standards.Core.Models.DTOs;
using Standards.Data.Repositories.Interfaces;

namespace Standards.Core.CQRS.Housings
{
    public class GetAll
    {
        public class Query : IRequest<IEnumerable<HousingDto>>
        {
            public Query()
            {
            }
        }

        public class QueryHandler : IRequestHandler<Query, IEnumerable<HousingDto>>
        {
            private readonly IRepository _repository;

            public QueryHandler(IRepository repository)
            {
                _repository = repository;
            }

            public async Task<IEnumerable<HousingDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var housings = await _repository.GetListAsync<HousingDto>(cancellationToken);

                return housings;
            }
        }
    }
}