using FluentValidation;
using MediatR;
using Standards.Core.CQRS.Common.Constants;
using Standards.Core.Models.Departments;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using Standards.Infrastructure.Services.Interfaces;
using Standards.Infrastructure.Validators;

namespace Standards.Core.CQRS.Departments
{
    public class GetById
    {
        public class Query(int id) : IRequest<Department>
        {
            public int Id { get; } = id;
        }

        public class QueryHandler(IRepository repository, ICacheService cacheService) : IRequestHandler<Query, Department>
        {
            public async Task<Department> Handle(Query request, CancellationToken cancellationToken)
            {
                var department = cacheService.GetById<Department>(Cache.Departments, request.Id);

                if (!cancellationToken.IsCancellationRequested && department is not null) return department;
                
                department = await repository.GetByIdAsync<Department>(request.Id, cancellationToken);

                return department;
            }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator(IRepository repository)
            {
                RuleLevelCascadeMode = CascadeMode.Stop;

                RuleFor(query => query.Id)
                    .GreaterThan(default(int))
                    .SetValidator(new IdValidator<Department>(repository));
            }
        }
    }
}