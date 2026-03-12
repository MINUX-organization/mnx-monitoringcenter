using MNX.MonitoringCenter.Management.Core.Mining.Miner.Configs;

namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders.MiningConfigs;

public abstract class MiningConfigBuilder<TBuilder, TEntity>
    where TBuilder : MiningConfigBuilder<TBuilder, TEntity>
    where TEntity : BaseMiningConfig, new()
{
    protected string? _additionalArguments = "default";
    protected string? _configFileContent = "default";
    protected readonly List<MiningCoinConfig> _coinConfigs = new(0);

    public TBuilder WithAdditionalArguments(string? arguments)
    {
        _additionalArguments = arguments;
        return (TBuilder)this;
    }

    public TBuilder WithConfigFileContent(string? configs)
    {
        _configFileContent = configs;
        return (TBuilder)this;
    }

    public TBuilder AddCoinConfig(Func<MiningCoinConfigBuilder, MiningCoinConfigBuilder>? configure = null)
    {
        var builder = new MiningCoinConfigBuilder();
        builder = configure?.Invoke(builder) ?? builder;
        _coinConfigs.Add(builder.Build());
        return (TBuilder)this;
    }

    public TBuilder AddCoinConfig(MiningCoinConfig coinConfig)
    {
        _coinConfigs.Add(coinConfig);
        return (TBuilder)this;
    }

    public TBuilder WithCoinConfigs(Func<List<MiningCoinConfig>> factory)
    {
        _coinConfigs.AddRange(factory());
        return (TBuilder)this;
    }

    public TBuilder WithCoinConfigs(IList<MiningCoinConfig> coinConfigs)
    {
        _coinConfigs.AddRange(coinConfigs);
        return (TBuilder)this;
    }

    public abstract TEntity Build();
}
