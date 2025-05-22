using Application.Abstractions.Cache;
using Application.UseCases.Common.Attributes;
using Domain.Constants;
using Domain.Models.Departments;
using Domain.Models.DTOs;
using FluentValidation;
using Infrastructure.Data.Repositories.Interfaces;
using Infrastructure.Validators;
using MediatR;

namespace Application.UseCases.Departments
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

                // var housings = repository.GetQueryable<Housing>()
                //     .Where(housing => housing.Departments
                //         .Any(d => d.Housings.Any(h => request.DepartmentDto.HousingIds.Contains(h.Id))))
                //     .ToList();
            
                var department = new Department
                {
                    Name = request.DepartmentDto.Name,
                    ShortName = request.DepartmentDto.ShortName,
                    Sectors = sectors,
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
                            .GreaterThan(0)
                            .SetValidator(new IdValidator<Department>(repository));

                        housing.RuleFor(department => department.Name)
                            .NotEmpty()
                            .MaximumLength(Lengths.EntityName);

                        housing.RuleFor(department => department.ShortName)
                            .NotEmpty()
                            .MaximumLength(Lengths.ShortName);

                        // housing.RuleFor(department => department.HousingIds)
                        //     .NotEmpty()
                        //     .ForEach(id => 
                        //         id.SetValidator(new IdValidator<Housing>(repository)));

                        housing.RuleFor(department => department.SectorIds)
                            .NotEmpty()
                            .ForEach(id => 
                                id.SetValidator(new IdValidator<Sector>(repository)));
                    });
            }
        }
    }
}
