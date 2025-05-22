using Application.Abstractions.Cache;
using Application.UseCases.Common.Attributes;
using Domain.Constants;
using Domain.Models;
using Domain.Models.DTOs;
using Domain.Models.Services;
using FluentValidation;
using Infrastructure.Data.Repositories.Interfaces;
using Infrastructure.Validators;
using MediatR;

namespace Application.UseCases.Services;

[TransactionScope]
public class Edit
{
    public class Query(ServiceDto serviceDto) : IRequest<int>
    {
        public ServiceDto ServiceDto { get; } = serviceDto;
    }

    public class QueryHandler(IRepository repository, ICacheService cacheService) : IRequestHandler<Query, int>
    {
        public async Task<int> Handle(Query request, CancellationToken cancellationToken)
        {
            var materials = repository.GetQueryable<Material>()
                .Where(material => request.ServiceDto.MaterialIds.Contains(material.Id))
                .ToList();

            var materialsQuantities = repository.GetQueryable<Quantity>()
                .Where(quantity => request.ServiceDto.MaterialsQuantityIds.Contains(quantity.Id))
                .ToList();

            var serviceType = await repository.GetByIdAsync<ServiceType>(request.ServiceDto.ServiceTypeId, cancellationToken);
            
            var service = new Service
            {
                Name = request.ServiceDto.Name,
                ShortName = request.ServiceDto.ShortName,
                ServiceType = serviceType,
                Materials = materials,
                MaterialsQuantities = materialsQuantities,
                Comments = request.ServiceDto.Comments
            };
                
            repository.Update(service);

            var result = await repository.SaveChangesAsync(cancellationToken);
                
            cacheService.Remove(Cache.Services);

            return result;
        }
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator(IRepository repository)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(query => query.ServiceDto)
                .NotEmpty()
                .ChildRules(service =>
                {
                    service.RuleFor(dto => dto.Id)
                        .GreaterThan(0)
                        .SetValidator(new IdValidator<Service>(repository));
                    
                    service.RuleFor(dto => dto.Name)
                        .NotEmpty()
                        .MaximumLength(Lengths.EntityName);

                    service.RuleFor(dto => dto.ShortName)
                        .NotEmpty()
                        .MaximumLength(Lengths.ShortName);
                    
                    service.RuleFor(dto => dto.ServiceTypeId)
                        .GreaterThan(0)
                        .SetValidator(new IdValidator<ServiceType>(repository));

                    service.RuleFor(dto => dto.MaterialIds)
                        .NotEmpty()
                        .ForEach(id => 
                            id.SetValidator(new IdValidator<Material>(repository)));

                    service.RuleFor(dto => dto.MaterialsQuantityIds)
                        .NotEmpty()
                        .ForEach(id => 
                            id.SetValidator(new IdValidator<Quantity>(repository)));
                });
        }
    }
}