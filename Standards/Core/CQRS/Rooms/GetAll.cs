using MediatR;
using Microsoft.EntityFrameworkCore;
using Standards.Core.Models.Departments;
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
                var rooms = await repository.GetListAsync<Room>(
                    query => query
                        .Include(r => r.WorkPlaces)
                        .Include(r => r.Persons),
                    cancellationToken);
                
                return rooms is null ? Array.Empty<Room>() : rooms;
            }
        }
    }
}