using FluentValidation;
using MediatR;
using Standards.Core.Constants;
using Standards.Core.CQRS.Common.Attributes;
using Standards.Core.CQRS.Common.Constants;
using Standards.Core.Models.Departments;
using Standards.Core.Models.DTOs;
using Standards.Core.Models.Housings;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using Standards.Infrastructure.Services.Interfaces;
using Standards.Infrastructure.Validators;

namespace Standards.Core.CQRS.Departments
{
    [TransactionScope]
    public class Edit
    {
        public class Query(DepartmentDto departmentDto) : IRequest<int>
        {
            public DepartmentDto DepartmentDto{ get; } = departmentDto;
        }

        public class QueryHandler(IRepository repository, ICacheService cacheService) : IRequestHandler<Query, int>
        {
            public async Task<int> Handle(Query request, CancellationToken cancellationToken)
            {
                var sectors = repository.GetQueryable<Sector>()
                    .Where(sector => sector.Department.Id == request.DepartmentDto.Id)
                    .ToList();

                var housings = repository.GetQueryable<Housing>()
                    .Where(housing => housing.Departments
                        .Any(d => d.Housings.Any(h => request.DepartmentDto.HousingIds.Contains(h.Id))))
                    .ToList();
            
                var department = new Department
                {
                    Name = request.DepartmentDto.Name,
                    ShortName = request.DepartmentDto.ShortName,
                    Sectors = sectors,
                    Housings = housings,
                    Comments = request.DepartmentDto.Comments
                };
                
                repository.Update(department);

                var result = await repository.SaveChangesAsync(cancellationToken);
                
                cacheService.Remove(Cache.Departments);

                return result;
            }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator(IRepository repository)
            {
                RuleLevelCascadeMode = CascadeMode.Stop;

                RuleFor(query => query.DepartmentDto)
                    .NotEmpty()
                    .ChildRules(housing =>
                    {
                        housing.RuleFor(department => department.Id)
                            .GreaterThan(default(int))
                            .SetValidator(new IdValidator<Department>(repository));

                        housing.RuleFor(department => department.Name)
                            .NotEmpty()
                            .Length(Lengths.EntityName);

                        housing.RuleFor(department => department.ShortName)
                            .NotEmpty()
                            .Length(Lengths.ShortName);

                        housing.RuleFor(department => department.HousingIds)
                            .NotEmpty()
                            .ForEach(id => 
                                id.SetValidator(new IdValidator<Housing>(repository)));

                        housing.RuleFor(department => department.SectorIds)
                            .NotEmpty()
                            .ForEach(id => 
                                id.SetValidator(new IdValidator<Sector>(repository)));
                    });
            }
        }
    }
}
