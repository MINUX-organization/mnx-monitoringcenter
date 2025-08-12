using FluentValidation;
using MNX.MonitoringCenter.Management.UseCases.Mining.Miner;
using MNX.MonitoringCenter.Management.UseCases.Mining.Pool;
using MNX.MonitoringCenter.Management.UseCases.Mining.Wallet;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Commands.CreateFlightSheet;

/// <summary>
/// Валидатор команды добавления полётного листа.
/// </summary>
public class CreateFlightSheetCommandValidator : AbstractValidator<CreateFlightSheetCommand>
{
    ///
    public CreateFlightSheetCommandValidator(IMiningConfigMapper mapper,
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
