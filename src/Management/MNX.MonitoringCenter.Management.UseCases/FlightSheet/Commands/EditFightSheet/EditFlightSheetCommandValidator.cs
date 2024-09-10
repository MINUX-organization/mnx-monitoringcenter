using FluentValidation;
using MNX.MonitoringCenter.Management.UseCases.FlightSheets.Commands.FlightSheets;
using MNX.MonitoringCenter.Management.UseCases.Miner;
using MNX.MonitoringCenter.Management.UseCases.Pool;
using MNX.MonitoringCenter.Management.UseCases.Wallet;

namespace MNX.MonitoringCenter.Management.UseCases.FlightSheets.Commands.EditFightSheet;

/// <summary>
/// Валидатор команды обновления полётного лита.
/// </summary>
public class EditFlightSheetCommandValidator : AbstractValidator<EditFlightSheetCommand>
{
    public EditFlightSheetCommandValidator(IWalletRepository walletRepository,
                                           IPoolRepository poolRepository,
                                           IMinerRepository minerRepository)
    {
        RuleFor(x => x.Model)
            .NotNull()
            .WithMessage("Flight sheet data is required!")
            .SetValidator(x => new FlightSheetModelValidator(x.UserId,
                                                             walletRepository,
                                                             poolRepository,
                                                             minerRepository));
    }
}
