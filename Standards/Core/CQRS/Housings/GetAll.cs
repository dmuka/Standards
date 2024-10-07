using MediatR;
using Standards.Core.Models.Housings;
using Standards.Infrastructure.Data.Repositories.Interfaces;

namespace Standards.Core.CQRS.Housings
{
    public class GetAll
    {
        public class Query : IRequest<IEnumerable<Housing>>
        {
        }

        public class QueryHandler(IRepository repository) : IRequestHandler<Query, IEnumerable<Housing>>
        {
            public async Task<IEnumerable<Housing>> Handle(Query request, CancellationToken cancellationToken)
            {
                var housings = await repository.GetListAsync<Housing>(cancellationToken);

                return housings is null ? Array.Empty<Housing>() : housings;
            }
        }
    }
}