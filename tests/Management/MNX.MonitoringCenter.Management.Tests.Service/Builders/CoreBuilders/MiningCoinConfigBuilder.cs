using MNX.MonitoringCenter.Management.Core.Mining.Miner.Configs;

namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders;

using Pool = Core.Mining.Pool;
using Wallet = Core.Mining.Wallet;

public class MiningCoinConfigBuilder
{
    private Guid _id = Guid.NewGuid();
    private Guid _walletId = Guid.NewGuid();
    private Wallet? _wallet = null;
    private Guid _poolId = Guid.NewGuid();
    private Pool? _pool = null;
    private string? _poolPassword = null;

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

    public MiningCoinConfigBuilder WithPool(Action<PoolBuilder>? configure = null)
    {
        var builder = new PoolBuilder();
        configure?.Invoke(builder);
        _pool = builder.Build();
        _poolId = _pool.Id;
        return this;
    }

    public MiningCoinConfigBuilder WithWallet(Action<WalletBuilder>? configure = null)
    {
        var builder = new WalletBuilder();
        configure?.Invoke(builder);
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
