using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Commands.Models.MiningConfig;

namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.ContractBuilders;

public class MiningCoinConfigInputModelBuilder
{
    private Guid _poolId = Guid.NewGuid();
    private Guid _walletId = Guid.NewGuid();
    private string? _poolPassword = null;

    public MiningCoinConfigInputModelBuilder WithPoolId(Guid poolId)
    {
        _poolId = poolId;
        return this;
    }

    public MiningCoinConfigInputModelBuilder WithWalletId(Guid walletId)
    {
        _walletId = walletId;
        return this;
    }

    public MiningCoinConfigInputModelBuilder WithPoolPassword(string? poolPassword)
    {
        _poolPassword = poolPassword;
        return this;
    }

    public MiningCoinConfigInputModel Build()
    {
        return new MiningCoinConfigInputModel
        {
            PoolId = _poolId,
            PoolPassword = _poolPassword,
            WalletId = _walletId,
        };
    }
}
