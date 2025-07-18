using Application.Abstractions.Cache;
using Application.Abstractions.Data;
using Application.Abstractions.Data.Validators;
using Application.UseCases.Common.Attributes;
using Domain;
using Domain.Models.Interfaces;
using FluentValidation;
using MediatR;

namespace Application.UseCases.Common.GenericCRUD;

[TransactionScope]
public class Delete 
{
    public class Command<T>(int id) : IRequest<int> where T : BaseEntity, ICacheable
    {
        public int Id { get; } = id;
    }

    public class CommandHandler<T>(
        IRepository repository, 
        ICacheService cacheService) : IRequestHandler<Command<T>, int> where T : BaseEntity, ICacheable
    {
        public async Task<int> Handle(Command<T> request, CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync<T>(request.Id, cancellationToken);
            
            if (entity == null) return 0;

            await repository.DeleteAsync(entity, cancellationToken);

            var result = await repository.SaveChangesAsync(cancellationToken);
            
            cacheService.Remove(T.GetCacheKey());

            return result;
        }
    }

    public class CommandValidator<T> : AbstractValidator<Command<T>> where T : BaseEntity, ICacheable
    {
        public CommandValidator(IRepository repository)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(query => query.Id)
                .GreaterThan(0);
        }
    }
}