using FluentValidation;
using MediatR;
using Standards.Core.Models.DTOs;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using Standards.Infrastructure.Validators;
using System.Data;
using System.Linq.Expressions;

namespace Standards.Core.CQRS.Housings
{
    public class Delete
    {
        public class Query : IRequest<int>
        {
            public Query(int id)
            {
                Id = id;
            }

            public int Id { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, int>
        {
            private readonly IRepository _repository;

            public QueryHandler(IRepository repository)
            {
                _repository = repository;
            }

            public async Task<int> Handle(Query request, CancellationToken cancellationToken)
            {
                using (var transaction = await _repository.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken))
                {
                    try
                    {
                        var housing = await _repository.GetByIdAsync<HousingDto>(request.Id, cancellationToken);

                        await _repository.DeleteAsync(housing, cancellationToken);

                        var result = await _repository.SaveChangesAsync(cancellationToken);

                        return result;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator(IRepository repository)
            {
                RuleLevelCascadeMode = CascadeMode.Stop;

                RuleFor(query => query.Id)
                    .GreaterThan(default(int))
                    .IdValidator<Query, HousingDto>(repository);
            }
        }
    }
}
