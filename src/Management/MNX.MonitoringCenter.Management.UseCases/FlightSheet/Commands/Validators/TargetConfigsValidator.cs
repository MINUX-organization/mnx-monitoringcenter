using FluentValidation;
using FluentValidation.Validators;
using MNX.MonitoringCenter.Management.UseCases.FlightSheet.Commands.Models.Target;
using MNX.MonitoringCenter.Management.UseCases.Pool;
using MNX.MonitoringCenter.Management.UseCases.Wallet;

namespace MNX.MonitoringCenter.Management.UseCases.FlightSheet.Commands.Validators;

/// <summary>
/// Валидатор конфигов в таргета.
/// </summary>
internal class TargetConfigsValidator : IAsyncPropertyValidator<FlightSheetTargetInputModel, List<FlightSheetTargetConfigInputModel>>
{
    private readonly IWalletRepository _walletRepository;

    private readonly IPoolRepository _poolRepository;

    private readonly Guid _userId;

    /// <inheritdoc/>
    public string Name { get => "TargetConfigs"; }

    internal TargetConfigsValidator(IWalletRepository walletRepository, IPoolRepository poolRepository, Guid userId)
    {
        _walletRepository = walletRepository;
        _poolRepository = poolRepository;
        _userId = userId;
    }

    public async Task<bool> IsValidAsync(
        ValidationContext<FlightSheetTargetInputModel> context, List<FlightSheetTargetConfigInputModel> value, CancellationToken cancellation)
    {
        List<Guid> coinIds = [];
        foreach (var config in value)
        {
            var wallet = await _walletRepository.GetAvailableById(config.WalletId, _userId);
            if (wallet == null)
            {
                context.AddFailure($"Wallet with id equaled {config.WalletId} was not found!");
            }

            var pool = await _poolRepository.GetAvailableById(config.PoolId, _userId);
            if (pool == null)
            {
                context.AddFailure($"Pool with id equaled {config.PoolId} was not found!");
            }

            config.TrimGapsInPoolPassword();
            if (config.PoolPassword != null && config.PoolPassword.Contains(' '))
            {
                context.AddFailure($"Pool password {config.PoolPassword} cannot contain space characters");
            }

            if (pool == null || wallet == null)
                continue;

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

        return true;
    }

    /// <inheritdoc/>
    public string GetDefaultMessageTemplate(string errorCode)
    {
        return "Passed flight sheet target configs were invalid!";
    }
}