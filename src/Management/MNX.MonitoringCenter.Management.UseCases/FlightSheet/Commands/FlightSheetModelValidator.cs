using FluentValidation;
using MNX.MonitoringCenter.Management.Core.Enums;
using MNX.MonitoringCenter.Management.Core.FlightSheet.Target;
using MNX.MonitoringCenter.Management.UseCases.FlightSheet.Commands.Models;
using MNX.MonitoringCenter.Management.UseCases.FlightSheet.Commands.Models.Target;
using MNX.MonitoringCenter.Management.UseCases.Miner;
using MNX.MonitoringCenter.Management.UseCases.Pool;
using MNX.MonitoringCenter.Management.UseCases.Wallet;

namespace MNX.MonitoringCenter.Management.UseCases.FlightSheet.Commands;

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
            RuleFor(model => model)
                .CustomAsync(async (model, context, _) =>
                {
                    var miner = await minerRepository.GetMinerById(model.MinerId);
                    if (miner == null)
                    {
                        context.AddFailure($"Miner with id equaled {model.MinerId} was not found!");
                        return;
                    }

                    if (miner.DeviceTypes.All(deviceType => DeviceTargetTypeConvertor.Convert(deviceType.DeviceType) != model.Type))
                    {
                        context.AddFailure("Target type is not supported by the miner.");
                    }

                    if (model.Type == FlightSheetTargetType.GPU && model.Configs.Count != (int)miner.MiningMode!)
                    {
                        context.AddFailure($"Invalid number of coins was passed for mining mode " +
                                           $"{miner.MiningMode}: {model.Configs.Count}.");
                    }
                });

            RuleFor(model => model.Configs)
                .NotEmpty()
                    .WithMessage("Configs are required!")
                .Must((target, configs) => (target.Type == FlightSheetTargetType.GPU && configs.Count <= 3) ||
                                           (target.Type == FlightSheetTargetType.CPU && configs.Count <= 1))
                .WithMessage(
                    "The number of configs for a flight sheet should not exceed 3 for a GPU and not exceed 1 for a CPU!");

            RuleFor(model => model.Configs)
                .NotNull()
                    .WithMessage("Config cannot be nullable!")
                .CustomAsync(async (configs, context, _) =>
                {
                    List<Guid> coinIds = [];
                    foreach (var config in configs)
                    {
                        var wallet = await walletRepository.GetAvailableById(config.WalletId, userId);
                        if (wallet == null)
                        {
                            context.AddFailure($"Wallet with id equaled {config.WalletId} was not found!");
                        }

                        var pool = await poolRepository.GetAvailableById(config.PoolId, userId);
                        if (pool == null)
                        {
                            context.AddFailure($"Pool with id equaled {config.PoolId} was not found!");
                        }

                        if (pool == null || wallet == null) return;

                        if (wallet.CryptocurrencyId != pool.CryptocurrencyId)
                        {
                            context.AddFailure(
                                $"Pool ({config.PoolId}) do not correlates with wallet ({config.WalletId}) by cryptocurrency!");
                        }

                        if (coinIds.Contains(pool.CryptocurrencyId))
                        {
                            context.AddFailure(
                                $"Cannot use the same cryptocurrency ({pool.CryptocurrencyId}) in different target configs.");
                        }
                        coinIds.Add(pool.CryptocurrencyId);
                    }
                });
        }
    }
}
