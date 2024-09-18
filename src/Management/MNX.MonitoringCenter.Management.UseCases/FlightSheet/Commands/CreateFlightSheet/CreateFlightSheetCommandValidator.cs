using FluentValidation;
using MNX.MonitoringCenter.Management.UseCases.Miner;
using MNX.MonitoringCenter.Management.UseCases.Pool;
using MNX.MonitoringCenter.Management.UseCases.Wallet;

namespace MNX.MonitoringCenter.Management.UseCases.FlightSheet.Commands.CreateFlightSheet;

/// <summary>
/// Валидатор команды добавления полётного листа.
/// </summary>
public class CreateFlightSheetCommandValidator : AbstractValidator<CreateFlightSheetCommand>
{
    public CreateFlightSheetCommandValidator(IMinerRepository minerRepository,
                                             IWalletRepository walletRepository,
                                             IPoolRepository poolRepository)
    {
        RuleFor(x => x.Model)
            .NotNull()
            .WithMessage("Flight sheet data is required!")
            .SetValidator(x => new FlightSheetModelValidator(x.UserId,
                                                             minerRepository,
                                                             walletRepository,
                                                             poolRepository));
    }
}
