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

namespace Standards.Core.CQRS.Departments;

[TransactionScope]
public class Create
{
    public class Query(DepartmentDto departmentDto) : IRequest<int>
    {
        public DepartmentDto DepartmentDto { get; } = departmentDto;
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
                Housings = housings
            };

            if (request.DepartmentDto.Comments is not null) department.Comments = request.DepartmentDto.Comments;
            
            await repository.AddAsync(department, cancellationToken);
            
            var result = await repository.SaveChangesAsync(cancellationToken);
            
            cacheService.Remove(Cache.Departments);

            return result;
        }
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(query => query.DepartmentDto)
                .NotEmpty()
                .ChildRules(filter =>
                {
                    filter.RuleFor(department => department.Name)
                        .NotEmpty()
                        .MaximumLength(Lengths.EntityName);

                    filter.RuleFor(department => department.ShortName)
                        .NotEmpty()
                        .MaximumLength(Lengths.ShortName);

                    filter.RuleFor(department => department.HousingIds)
                        .NotEmpty();

                    filter.RuleFor(department => department.SectorIds)
                        .NotEmpty();
                });
        }
    }

}