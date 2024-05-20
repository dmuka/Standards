using MediatR;
using Standards.Core.CQRS.Common.Attributes;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using System.Data;

namespace Standards.Infrastructure.Mediatr
{
    namespace Standards.Core.CQRS.Common.Behaviors
    {
        public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        {
            private readonly IRepository _repository;

            public TransactionBehavior(IRepository repository)
            {
                _repository = repository;
            }

            public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
            {
                var requestType = request.GetType();
                var hasTransactionScope = Attribute.IsDefined(requestType, typeof(TransactionScopeAttribute));

                if (!hasTransactionScope)
                {
                    return await next();
                }

                using (var transaction = await _repository.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken))
                {
                    try
                    {
                        var response = await next();
                        await _repository.SaveChangesAsync(cancellationToken);
                        transaction.Commit();

                        return response;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}