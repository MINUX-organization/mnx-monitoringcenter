using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Commands.Models.MiningConfig;

namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.ContractBuilders.MiningConfigInputModels;

public abstract class MiningConfigInputModelBuilder<TBuilder, TEntity>
    where TBuilder : MiningConfigInputModelBuilder<TBuilder, TEntity>
    where TEntity : MiningConfigInputModel, new()
{
    protected readonly List<MiningCoinConfigInputModel> _coinConfigs = new(0);
    protected string? _additionalArguments = null;
    protected string? _configFileContent = null;

    public TBuilder WithAdditionalArguments(string? additionalArguments)
    {
        _additionalArguments = additionalArguments;
        return (TBuilder)this;
    }

    public TBuilder WithConfigFileContent(string? configFileContent)
    {
        _configFileContent = configFileContent;
        return (TBuilder)this;
    }

    public TBuilder AddCoinConfig(
        Func<MiningCoinConfigInputModelBuilder, MiningCoinConfigInputModelBuilder>? configure = null)
    {
        var builder = new MiningCoinConfigInputModelBuilder();
        builder = configure?.Invoke(builder) ?? builder;
        _coinConfigs.Add(builder.Build());
        return (TBuilder)this;
    }

    public abstract MiningConfigInputModel Build();
}
