using MNX.MonitoringCenter.Management.Core.Mining.Miner.Configs;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Tests.Builders.MiningConfigs;

public abstract class MiningConfigBuilder<TBuilder, TEntity>
    where TBuilder : MiningConfigBuilder<TBuilder, TEntity>
    where TEntity : BaseMiningConfig, new()
{
    protected string _additionalArguments = "default";
    protected string _configFileContent = "default";
    protected readonly List<MiningCoinConfig> _coinConfigs = new(3);

    public TBuilder WithAdditionalArguments(string arguments)
    {
        _additionalArguments = arguments;
        return (TBuilder)this;
    }

    public TBuilder WithConfigFileContent(string configs)
    {
        _configFileContent = configs;
        return (TBuilder)this;
    }

    public TBuilder AddCoinConfig(Action<MiningCoinConfigBuilder> configure)
    {
        var builder = new MiningCoinConfigBuilder();
        configure(builder);
        if (_coinConfigs.Count >= 3)
            throw new IndexOutOfRangeException("You cannot add more than 3 MiningCoinConfigs");
        _coinConfigs.Add(builder.Build());
        return (TBuilder)this;
    }

    protected void ApplyBasesProperties(TEntity entity)
    {
        entity.AdditionalArguments = _additionalArguments;
        entity.ConfigFileContent = _configFileContent;
        entity.CoinConfigs = _coinConfigs.ToList();
    }

    public abstract TEntity Build();
}
