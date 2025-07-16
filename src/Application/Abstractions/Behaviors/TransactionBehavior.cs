using Application.Abstractions.Data;
using Application.UseCases.Common.Attributes;
using MediatR;

namespace Application.Abstractions.Behaviors;

public class TransactionBehavior<TRequest, TResponse>(IUnitOfWork unitOfWork)
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

        await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var response = await next(cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);

            return response;
        }
        catch
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}