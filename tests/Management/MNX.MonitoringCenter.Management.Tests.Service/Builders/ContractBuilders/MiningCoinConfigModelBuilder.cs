using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Management.Contracts.FlightSheet.MiningConfigs;

namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.ContractBuilders;

public class MiningCoinConfigModelBuilder
{
    private WalletModel? _wallet = null;
    private PoolModel? _pool = null;
    private string? _poolPassword = null;

    public MiningCoinConfigModelBuilder WithWallet(Func<WalletModelBuilder, WalletModelBuilder> configure)
    {
        var builder = new WalletModelBuilder();
        builder = configure(builder);
        _wallet = builder.Build();
        return this;
    }

    public MiningCoinConfigModelBuilder WithPool(Action<PoolModelBuilder> configure)
    {
        var builder = new PoolModelBuilder();
        configure(builder);
        _pool = builder.Build();
        return this;
    }

    public MiningCoinConfigModelBuilder WithPoolPassword(string? poolPassword)
    {
        _poolPassword = poolPassword;
        return this;
    }

    public MiningCoinConfigModel Build()
    {
        return new MiningCoinConfigModel
        {
            Wallet = _wallet ?? new WalletModelBuilder().Build(),
            Pool = _pool ?? new PoolModelBuilder().Build(),
            PoolPassword = _poolPassword,
        };
    }
}
