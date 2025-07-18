using Application.UseCases.DTOs;
using Core.Results;
using Domain.Aggregates.Housings;
using MediatR;

namespace Application.UseCases.Housings;

public class EditHousing
{
    public class Command(HousingDto2 housing) : IRequest<Result>
    {
        public HousingDto2 HousingDto { get; set; } = housing;
    }

    public class CommandHandler(IHousingRepository repository) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var isHousingExist = await repository.ExistsAsync(command.HousingDto.Id, cancellationToken);
            
            if (!isHousingExist) return Result.Failure(HousingErrors.NotFound(command.HousingDto.Id));
            
            var existingHousing = await repository.GetByIdAsync(command.HousingDto.Id, cancellationToken: cancellationToken);
            
            repository.Update(existingHousing!);

            return Result.Success();
        }
    }
}