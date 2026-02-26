using MNX.MonitoringCenter.Management.Core.Mining.Miner.Configs;

namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders.MiningConfigs;

public abstract class MiningConfigBuilder<TBuilder, TEntity>
    where TBuilder : MiningConfigBuilder<TBuilder, TEntity>
    where TEntity : BaseMiningConfig, new()
{
    protected string? _additionalArguments = "default";
    protected string? _configFileContent = "default";
    protected readonly List<MiningCoinConfig> _coinConfigs = new(0);

    protected void ApplyBaseProperties(TEntity entity)
    {
        entity.AdditionalArguments = _additionalArguments;
        entity.ConfigFileContent = _configFileContent;
        entity.CoinConfigs = _coinConfigs.ToList();
    }

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

    public TBuilder AddCoinConfig(Action<MiningCoinConfigBuilder> configure)
    {
        var builder = new MiningCoinConfigBuilder();
        configure(builder);
        _coinConfigs.Add(builder.Build());
        return (TBuilder)this;
    }

    public abstract TEntity Build();
}
