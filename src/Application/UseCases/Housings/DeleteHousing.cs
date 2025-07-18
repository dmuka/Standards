using Core.Results;
using Domain.Aggregates.Housings;
using MediatR;

namespace Application.UseCases.Housings;

public class DeleteHousing
{
    public class Command(HousingId housingId) : IRequest<Result>
    {
        public HousingId HousingId { get; set; } = housingId;
    }
    
    public class CommandHandler(IHousingRepository repository) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var isHousingExist = await repository.ExistsAsync(command.HousingId, cancellationToken);
            
            if (!isHousingExist) return Result.Failure<int>(HousingErrors.NotFound(command.HousingId));
            
            var existingHousing = await repository.GetByIdAsync(command.HousingId, cancellationToken: cancellationToken);
            
            repository.Remove(existingHousing!);

            return Result.Success();
        }
    }
}