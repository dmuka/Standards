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

namespace Standards.Core.CQRS.Housings;

[TransactionScope]
public class Create
{
    public class Query(HousingDto housingDto) : IRequest<int>
    {
        public HousingDto HousingDto { get; } = housingDto;
    }

    public class QueryHandler(IRepository repository, ICacheService cacheService) : IRequestHandler<Query, int>
    {
        public async Task<int> Handle(Query request, CancellationToken cancellationToken)
        {
            var housing = new Housing()
            {
                Name = request.HousingDto.Name,
                ShortName = request.HousingDto.ShortName,
                FloorsCount = request.HousingDto.FloorsCount,
                Address = request.HousingDto.Address
            };

            if (request.HousingDto.Comments is not null) housing.Comments = request.HousingDto.Comments;
            
            await repository.AddAsync(housing, cancellationToken);
            
            var result = await repository.SaveChangesAsync(cancellationToken);
            
            cacheService.Remove(Cache.Housings);

            return result;
        }
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(query => query.HousingDto)
                .NotEmpty()
                .ChildRules(filter =>
                {
                    filter.RuleFor(housing => housing.Name)
                        .NotEmpty()
                        .Length(Lengths.EntityName);

                    filter.RuleFor(housing => housing.ShortName)
                        .NotEmpty()
                        .Length(Lengths.ShortName);

                    filter.RuleFor(housing => housing.FloorsCount)
                        .GreaterThan(default(int));

                    filter.RuleFor(housing => housing.Address)
                        .NotEmpty();
                });
        }
    }

}