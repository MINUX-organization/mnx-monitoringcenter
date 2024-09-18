using FluentValidation;
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
            .Must(targets => targets.Where(x => x.Type == FlightSheetTargetType.CPU).Count() <= 1)
                .WithMessage("The number of CPU targets should not exceed 1")
            .Must(targets => targets.Where(x => x.Type == FlightSheetTargetType.GPU).Count() <= 1)
                .WithMessage("The number of GPU targets should not exceed 1");

        RuleForEach(model => model.Targets)
            .NotNull()
            .WithMessage("Target is cannot nullable!")
            .SetValidator(model => new FlightSheetTargetModelValidator(userId,
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
                .NotEmpty()
                    .WithMessage("Miner is required!")
                .MustAsync(async (model, miner, cancellationToken) => await minerRepository.Exists(miner, cancellationToken))
                    .WithMessage(model => $"Miner with id {model.MinerId} wasn`t found!");

            RuleFor(model => model.Configs)
                .NotEmpty()
                .WithMessage("Configs is required!")
                .Must((target, configs) => (target.Type == FlightSheetTargetType.GPU && configs.Count <= 3) ||
                                           (target.Type == FlightSheetTargetType.CPU && configs.Count <= 1))
                .WithMessage("The number of configs for a flight sheet should not exceed 3 for a GPU and not exceed 1 for a CPU!");

            RuleForEach(model => model.Configs)
                .NotNull()
                .WithMessage("Config is cannot nullable!")
                .SetValidator(x => new MiningConfigValidator(userId, walletRepository, poolRepository));
        }

        /// <summary>
        /// Валидатор конфига.
        /// </summary>
        private class MiningConfigValidator : AbstractValidator<FlightSheetTargetConfigInputModel>
        {
            internal MiningConfigValidator(Guid userId,
                                           IWalletRepository walletRepository,
                                           IPoolRepository poolRepository)
            {
                RuleFor(config => config.WalletId)
                    .MustAsync(async (config, walletId, cancellationToken)
                                => await walletRepository.GetAvailableById(walletId, userId) is not null)
                    .WithMessage(config => $"Wallet with id is equaled {config.WalletId} was not found!");

                RuleFor(config => config.PoolId)
                    .MustAsync(async (config, poolId, cancellationToken)
                                => await poolRepository.GetAvailableById(poolId, userId) is not null)
                    .WithMessage(config => $"Pool with id is equaled {config.PoolId} was not found!");

                RuleFor(config => new { config.PoolId, config.WalletId })
                    .MustAsync(async (config, ids, cancellationToken) =>
                    {
                        var wallet = await walletRepository.GetAvailableById(ids.WalletId, userId);
                        var pool = await poolRepository.GetAvailableById(config.PoolId, userId);

                        if (wallet is not null && pool is not null)
                        {
                            return wallet!.CryptocurrencyId == pool!.CryptocurrencyId;
                        }

                        return false;
                    })
                    .WithMessage("Pool don`t correlates with wallet by cryptocurrency!");

                // todo: проверка соответствия комбинации монет.
            }
        }
    }
}
