using MediatR;
using Standards.Core.Models.DTOs;
using Standards.Infrastructure.Data.Repositories.Interfaces;

namespace Standards.Core.CQRS.Rooms
{
    public class GetAll
    {
        public class Query : IRequest<IEnumerable<RoomDto>>
        {
        }

        public class QueryHandler(IRepository repository) : IRequestHandler<Query, IEnumerable<RoomDto>>
        {
            public async Task<IEnumerable<RoomDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var rooms = await repository.GetListAsync<RoomDto>(cancellationToken);
                
                return rooms is null ? Array.Empty<RoomDto>() : rooms;
            }
        }
    }
}