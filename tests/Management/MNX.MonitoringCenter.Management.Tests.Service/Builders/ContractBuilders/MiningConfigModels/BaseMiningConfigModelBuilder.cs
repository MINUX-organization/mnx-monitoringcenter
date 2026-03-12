using MNX.MonitoringCenter.Management.Contracts.FlightSheet.MiningConfigs;

namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.ContractBuilders.MiningConfigModels;

public abstract class BaseMiningConfigModelBuilder<TBuilder, TEntity>
    where TBuilder : BaseMiningConfigModelBuilder<TBuilder, TEntity>
    where TEntity : BaseMiningConfigModel, new()
{
    protected string? _additionalArguments = null;
    protected string? _configFileContent = null;
    protected readonly List<MiningCoinConfigModel> _coinConfigs = new(0);

    public TBuilder WithAdditionalArguments(string? additionalArgumetns)
    {
        _additionalArguments = additionalArgumetns;
        return (TBuilder)this;
    }

    public TBuilder WithConfigFileContent(string? configFileContent)
    {
        _configFileContent = configFileContent;
        return (TBuilder)this;
    }

    public TBuilder AddCoinConfig(Func<MiningCoinConfigModelBuilder, MiningCoinConfigModelBuilder>? configure = null)
    {
        var builder = new MiningCoinConfigModelBuilder();
        builder = configure?.Invoke(builder) ?? builder;
        _coinConfigs.Add(builder.Build());
        return (TBuilder)this;
    }

    public TBuilder WithCoinConfigs(Func<List<MiningCoinConfigModel>> factory)
    {
        _coinConfigs.AddRange(factory());
        return (TBuilder)this;
    }

    public abstract BaseMiningConfigModel Build();
}
