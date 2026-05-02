using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Management.Contracts.FlightSheet.MiningConfigs;

namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.ContractBuilders;

public class MiningCoinConfigModelBuilder
{
    protected WalletModel? _wallet = null;
    protected PoolModel? _pool = null;
    protected string? _poolPassword = null;

    public MiningCoinConfigModelBuilder WithWallet(Func<WalletModelBuilder, WalletModelBuilder>? configure = null)
    {
        var builder = new WalletModelBuilder();
        builder = configure?.Invoke(builder) ?? builder;
        _wallet = builder.Build();
        return this;
    }

    public MiningCoinConfigModelBuilder WithPool(Func<PoolModelBuilder, PoolModelBuilder>? configure = null)
    {
        var builder = new PoolModelBuilder();
        builder = configure?.Invoke(builder) ?? builder;
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
