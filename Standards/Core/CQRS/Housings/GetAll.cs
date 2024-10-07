using MediatR;
using Microsoft.EntityFrameworkCore;
using Standards.Core.Models.DTOs;
using Standards.Core.Models.Housings;
using Standards.Infrastructure.Data.Repositories.Interfaces;

namespace Standards.Core.CQRS.Housings
{
    public class GetAll
    {
        public class Query : IRequest<IList<HousingDto>>
        {
        }

        public class QueryHandler(IRepository repository) : IRequestHandler<Query, IList<HousingDto>>
        {
            public async Task<IList<HousingDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var housings = await repository.GetListAsync<Housing>(
                    query => query
                        .Include(h => h.Departments)
                        .Include(h => h.Rooms), 
                    cancellationToken);

                if (housings is null) return [];
                
                var dtos = housings
                    .Select(h => new HousingDto()
                    {
                        Id = h.Id,
                        Name = h.Name,
                        ShortName = h.ShortName,
                        Address = h.Address,
                        FloorsCount = h.FloorsCount,
                        Comments = h.Comments,
                        DepartmentIds = h.Departments.Select(d => d.Id).ToList(),
                        RoomIds = h.Rooms.Select(r => r.Id).ToList()
                    }).ToList();

                return dtos;
            }
        }
    }
}