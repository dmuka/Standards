using FluentValidation;
using MediatR;
using Standards.Core.CQRS.Common.Constants;
using Standards.Core.Models.Housings;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using Standards.Infrastructure.Services.Interfaces;
using Standards.Infrastructure.Validators;

namespace Standards.Core.CQRS.Rooms
{
    public class GetById
    {
        public class Query(int id) : IRequest<Room>
        {
            public int Id { get; set; } = id;
        }

        public class QueryHandler(IRepository repository, ICacheService cacheService) : IRequestHandler<Query, Room>
        {
            public async Task<Room> Handle(Query request, CancellationToken cancellationToken)
            {
                var room = cacheService.GetById<Room>(Cache.Housings, request.Id);

                if (room is not null) return room;
                
                room = await repository.GetByIdAsync<Room>(request.Id, cancellationToken);

                return room;
            }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator(IRepository repository)
            {
                RuleLevelCascadeMode = CascadeMode.Stop;

                RuleFor(query => query.Id)
                    .GreaterThan(default(int))
                    .SetValidator(new IdValidator<Room>(repository));
            }
        }
    }
}