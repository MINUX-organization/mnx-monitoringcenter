using MNX.MonitoringCenter.Management.Contracts.Overclocking;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.ContractBuilders.Overclockings;
using MNX.MonitoringCenter.Management.UseCases.Overclocking.Presets.Commands;

namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.ContractBuilders;

public class PresetInputModelBuilder
{
    private static int _counter = 1;

    protected string _name = $"PresetInputModelName_{_counter}";
    protected string _deviceName = $"DeviceName_{_counter++}";
    protected IOverclockingModel? _overclocking = null;

    public PresetInputModelBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public PresetInputModelBuilder WithDeviceName(string deviceName)
    {
        _deviceName = deviceName;
        return this;
    }

    public PresetInputModelBuilder WithOverclocking(Func<IOverclockingModel> factory)
    {
        _overclocking = factory();
        return this;
    }

    public PresetInputModel Build()
    {
        return new PresetInputModel(
            _name,
            _deviceName,
            _overclocking ?? new CpuOverclockingModelBuilder().Build()
        );
    }
}
