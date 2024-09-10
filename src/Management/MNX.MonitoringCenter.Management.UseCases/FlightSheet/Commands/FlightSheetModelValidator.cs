using FluentValidation;
using MNX.MonitoringCenter.Management.UseCases.FlightSheets.Commands.Models;
using MNX.MonitoringCenter.Management.UseCases.Miner;
using MNX.MonitoringCenter.Management.UseCases.Pool;
using MNX.MonitoringCenter.Management.UseCases.Wallet;

namespace MNX.MonitoringCenter.Management.UseCases.FlightSheets.Commands.FlightSheets;

/// <summary>
/// Валидатор модели полётного листа.
/// </summary>
internal class FlightSheetModelValidator : AbstractValidator<FlightSheetInputModel>
{
    internal FlightSheetModelValidator(Guid userId,
                                       IWalletRepository walletRepository,
                                       IPoolRepository poolRepository,
                                       IMinerRepository minerRepository)
    {
        RuleFor(model => model.Name)
            .Must(name =>! string.IsNullOrWhiteSpace(name))
            .WithMessage("Name is required!");

        RuleFor(model => model.Miner)
            .Must(miner => ! string.IsNullOrWhiteSpace(miner))
            .WithMessage("Miner wasn`t found!")
            .MustAsync(async (model, miner, cancellationToken) => await minerRepository.Exists(miner, cancellationToken))
            .WithMessage(model => $"Miner with name {model.Miner} wasn`t found!");

        RuleFor(model => model.Configs)
            .NotEmpty()
            .WithMessage("Configs is required!");

        RuleForEach(model => model.Configs)
            .NotEmpty()
            .WithMessage("Config is cannot nullable!")
            .SetValidator(x => new MiningConfigValidator(userId, walletRepository, poolRepository));
    }

    /// <summary>
    /// Валидатор конфига.
    /// </summary>
    private class MiningConfigValidator : AbstractValidator<FlightSheetConfigInputModel>
    {
        internal MiningConfigValidator(Guid userId, IWalletRepository walletRepository, IPoolRepository poolRepository)
        {
            RuleFor(config => config.WalletId)
                .MustAsync(async (config, walletId, CancellationToken)
                            => await walletRepository.GetAvailableById(walletId, userId) is not null)
                .WithMessage(config => $"Wallet with id is equaled {config.WalletId} was not found!");

            RuleFor(config => config.PoolId)
                .MustAsync(async (config, poolId, CancellationToken)
                            => await poolRepository.GetAvailableById(poolId, userId) is not null)
                .WithMessage(config => $"Pool with id is equaled {config.PoolId} was not found!");

            RuleFor(config => new { config.PoolId, config.WalletId })
                .MustAsync(async (config, ids, cancellationToken) =>
                {
                    var wallet = await walletRepository.GetAvailableById(ids.WalletId, userId);
                    var pool = await poolRepository.GetAvailableById(config.PoolId, userId);

                    return wallet!.CryptocurrencyId == pool!.CryptocurrencyId;
                })
                .WithMessage("Pool don`t correlates with wallet by cryptocurrency!");

            // todo: проверка соответствия комбинации монет.
        }
    }
}
