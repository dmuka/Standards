using MediatR;
using Standards.Core.CQRS.Common.Attributes;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using System.Data;

namespace Standards.Infrastructure.Mediatr
{
    namespace Standards.Core.CQRS.Common.Behaviors
    {
        public class TransactionBehavior<TRequest, TResponse>(IRepository repository)
            : IPipelineBehavior<TRequest, TResponse>
            where TRequest : notnull
        {
            public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
            {
                var requestType = request.GetType();
                var hasTransactionScope = Attribute.IsDefined(requestType, typeof(TransactionScopeAttribute));

                if (!hasTransactionScope)
                {
                    return await next();
                }

                await using var transaction = await repository.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);
                try
                {
                    var response = await next();
                    await repository.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);

                    return response;
                }
                catch
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }
        }
    }
}