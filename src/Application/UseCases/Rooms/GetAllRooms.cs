using Domain.Aggregates.Rooms;
using MediatR;

namespace Application.UseCases.Rooms;

public class GetAllRooms
{
    public class Query : IRequest<IList<Room>>;
    
    public class QueryHandler(IRoomRepository repository) : IRequestHandler<Query, IList<Room>>
    {
        public async Task<IList<Room>> Handle(Query request, CancellationToken cancellationToken)
        {
            var rooms = await repository.GetAllAsync(cancellationToken);

            return rooms;
        }
    }
}