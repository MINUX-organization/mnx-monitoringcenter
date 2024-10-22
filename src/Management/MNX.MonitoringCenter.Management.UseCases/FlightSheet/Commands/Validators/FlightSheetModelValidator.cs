using FluentValidation;
using MNX.MonitoringCenter.Management.Core.FlightSheet.Target;
using MNX.MonitoringCenter.Management.UseCases.FlightSheet.Commands.Models;
using MNX.MonitoringCenter.Management.UseCases.FlightSheet.Commands.Models.Target;
using MNX.MonitoringCenter.Management.UseCases.Miner;
using MNX.MonitoringCenter.Management.UseCases.Pool;
using MNX.MonitoringCenter.Management.UseCases.Wallet;

namespace MNX.MonitoringCenter.Management.UseCases.FlightSheet.Commands.Validators;

/// <summary>
/// Валидатор модели полётного листа.
/// </summary>
internal class FlightSheetModelValidator : AbstractValidator<FlightSheetInputModel>
{
    public FlightSheetModelValidator(Guid userId,
                                     IMinerRepository minerRepository,
                                     IWalletRepository walletRepository,
                                     IPoolRepository poolRepository)
    {
        RuleFor(model => model.Name)
            .Must(name => !string.IsNullOrWhiteSpace(name))
                .WithMessage("Name is required!");

        RuleFor(model => model.Targets)
            .NotEmpty()
                .WithMessage("Flight sheet targets is required!")
            .Must(targets => targets.Count(x => x.Type == FlightSheetTargetType.CPU) <= 1)
                .WithMessage("The number of CPU targets should not exceed 1")
            .Must(targets => targets.Count(x => x.Type == FlightSheetTargetType.GPU) <= 1)
                .WithMessage("The number of GPU targets should not exceed 1");

        RuleForEach(model => model.Targets)
            .NotNull()
                .WithMessage("Target is cannot nullable!")
            .SetValidator(_ => new FlightSheetTargetModelValidator(userId,
                                                                   minerRepository,
                                                                   walletRepository,
                                                                   poolRepository));
    }

    /// <summary>
    /// Валидатор модели таргета полётного листа.
    /// </summary>
    private class FlightSheetTargetModelValidator : AbstractValidator<FlightSheetTargetInputModel>
    {
        internal FlightSheetTargetModelValidator(Guid userId,
                                                 IMinerRepository minerRepository,
                                                 IWalletRepository walletRepository,
                                                 IPoolRepository poolRepository)
        {
            RuleFor(model => model.MinerId)
                .SetAsyncValidator(new TargetMinerValidator(minerRepository));

            RuleFor(model => model.Configs)
                .NotEmpty()
                    .WithMessage("Configs are required!")
                .Must((target, configs) => target.Type == FlightSheetTargetType.GPU && configs.Count <= 3 ||
                                           target.Type == FlightSheetTargetType.CPU && configs.Count <= 1)
                .WithMessage(
                    "The number of configs for a flight sheet should not exceed 3 for a GPU and not exceed 1 for a CPU!");

            RuleFor(model => model.Configs)
                .NotNull()
                    .WithMessage("Config cannot be nullable!")
                .SetAsyncValidator(new TargetConfigsValidator(walletRepository, poolRepository, userId));
        }
    }
}
