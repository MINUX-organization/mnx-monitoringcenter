using AutoMapper;
using FluentValidation;
using MNX.MonitoringCenter.Management.UseCases.Mining.Miner;
using MNX.MonitoringCenter.Management.UseCases.Mining.Pool;
using MNX.MonitoringCenter.Management.UseCases.Mining.Wallet;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Commands.EditFightSheet;

/// <summary>
/// Валидатор команды обновления полётного лита.
/// </summary>
public class EditFlightSheetCommandValidator : AbstractValidator<EditFlightSheetCommand>
{
    public EditFlightSheetCommandValidator(IMapper mapper,
                                           IMinerRepository minerRepository,
                                           IWalletRepository walletRepository,
                                           IPoolRepository poolRepository)
    {
        RuleFor(x => x.Model)
            .NotNull()
            .WithMessage("Flight sheet data is required!")
            .SetValidator(x => new FlightSheetModelValidator(x.UserId,
                                                             mapper,
                                                             minerRepository,
                                                             walletRepository,
                                                             poolRepository));
    }
}
