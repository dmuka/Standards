using MediatR;
using Standards.Core.Models.DTOs;
using Standards.Infrastructure.Data.Repositories.Interfaces;

namespace Standards.Core.CQRS.Housings
{
    public class GetAll
    {
        public class Query : IRequest<IEnumerable<HousingDto>>
        {
        }

        public class QueryHandler(IRepository repository) : IRequestHandler<Query, IEnumerable<HousingDto>>
        {
            public async Task<IEnumerable<HousingDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var housings = await repository.GetListAsync<HousingDto>(cancellationToken);

                return housings is null ? Array.Empty<HousingDto>() : housings;
            }
        }
    }
}