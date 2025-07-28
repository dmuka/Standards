using Core.Results;
using Domain.Aggregates.Rooms;
using MediatR;

namespace Application.UseCases.Rooms;

public class GetRoomById
{
    public class Query(RoomId roomId) : IRequest<Result<Room>>
    {
        public RoomId RoomId { get; } = roomId;
    }
    
    public class QueryHandler(IRoomRepository repository) : IRequestHandler<Query, Result<Room>>
    {
        public async Task<Result<Room>> Handle(Query query, CancellationToken cancellationToken)
        {
            var isRoomExist = await repository.ExistsAsync(query.RoomId, cancellationToken);
            
            if (!isRoomExist) return Result.Failure<Room>(RoomErrors.NotFound(query.RoomId));
            
            var room = await repository.GetByIdAsync(query.RoomId, cancellationToken);

            return room;
        }
    }
}