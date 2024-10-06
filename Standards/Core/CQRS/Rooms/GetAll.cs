using MediatR;
using Standards.Core.Models.DTOs;
using Standards.Core.Models.Housings;
using Standards.Infrastructure.Data.Repositories.Interfaces;

namespace Standards.Core.CQRS.Rooms
{
    public class GetAll
    {
        public class Query : IRequest<IEnumerable<Room>>
        {
        }

        public class QueryHandler(IRepository repository) : IRequestHandler<Query, IEnumerable<Room>>
        {
            public async Task<IEnumerable<Room>> Handle(Query request, CancellationToken cancellationToken)
            {
                var rooms = await repository.GetListAsync<Room>(cancellationToken);
                
                return rooms is null ? Array.Empty<Room>() : rooms;
            }
        }
    }
}