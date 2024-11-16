using AutoMapper;
using FluentValidation;
using MNX.MonitoringCenter.Management.UseCases.Miner;
using MNX.MonitoringCenter.Management.UseCases.Pool;
using MNX.MonitoringCenter.Management.UseCases.Wallet;

namespace MNX.MonitoringCenter.Management.UseCases.FlightSheet.Commands.EditFightSheet;

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
