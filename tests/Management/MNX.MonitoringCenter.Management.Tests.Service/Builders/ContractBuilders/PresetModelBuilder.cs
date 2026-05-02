using MNX.MonitoringCenter.Management.Contracts.Overclocking;
using MNX.MonitoringCenter.Management.Contracts.Presets;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.ContractBuilders.Overclockings;

namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.ContractBuilders;

public class PresetModelBuilder
{
    private static int _counter = 1;

    protected Guid _id = Guid.NewGuid();
    protected string _name = $"PresetModelName_{_counter}";
    protected string _deviceName = $"DeviceName_{_counter++}";
    protected IOverclockingModel? _overclocking = null;

    public PresetModelBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public PresetModelBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public PresetModelBuilder WithDeviceName(string deviceName)
    {
        _deviceName = deviceName;
        return this;
    }

    public PresetModelBuilder WithOverclocking(Func<IOverclockingModel> factory)
    {
        _overclocking = factory();
        return this;
    }

    public PresetModel Build()
    {
        return new PresetModel
        {
            Id = _id,
            Name = _name,
            DeviceName = _deviceName,
            Overclocking = _overclocking ??
                new CpuOverclockingModelBuilder().Build(),
        };
    }
}
