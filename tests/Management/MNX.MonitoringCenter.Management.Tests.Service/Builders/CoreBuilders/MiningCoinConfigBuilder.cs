using MNX.MonitoringCenter.Management.Core.Mining.Miner.Configs;

namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders;

using Pool = Core.Mining.Pool;
using Wallet = Core.Mining.Wallet;

public class MiningCoinConfigBuilder
{
    protected Guid _id = Guid.NewGuid();
    protected Guid _walletId = Guid.NewGuid();
    protected Wallet? _wallet = null;
    protected Guid _poolId = Guid.NewGuid();
    protected Pool? _pool = null;
    protected string? _poolPassword = null;

    public MiningCoinConfigBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public MiningCoinConfigBuilder WithPoolPassword(string? poolPassword)
    {
        _poolPassword = poolPassword;
        return this;
    }

    public MiningCoinConfigBuilder WithPool(Func<PoolBuilder, PoolBuilder>? configure = null)
    {
        var builder = new PoolBuilder();
        builder = configure?.Invoke(builder) ?? builder;
        _pool = builder.Build();
        _poolId = _pool.Id;
        return this;
    }

    public MiningCoinConfigBuilder WithWallet(Func<WalletBuilder, WalletBuilder>? configure = null)
    {
        var builder = new WalletBuilder();
        builder = configure?.Invoke(builder) ?? builder;
        _wallet = builder.Build();
        _walletId = _wallet.Id;
        return this;
    }

    public MiningCoinConfig Build()
    {
        return new MiningCoinConfig()
        {
            Id = _id,
            WalletId = _walletId,
            Wallet = _wallet,
            PoolId = _poolId,
            Pool = _pool,
            PoolPassword = _poolPassword,
        };
    }
}
