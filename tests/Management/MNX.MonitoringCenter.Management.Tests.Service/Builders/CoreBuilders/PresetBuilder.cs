using MNX.MonitoringCenter.Management.Core.Overclocking;

namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders;

public class PresetBuilder
{
    private static int _counter = 1;

    protected Guid _id = Guid.NewGuid();
    protected string _name = $"PresetName_{_counter}";
    protected string _deviceName = $"DeviceName_{_counter++}";
    protected Guid _ownerId = Guid.NewGuid();
    protected bool _isVisible = false;
    protected Guid _overclockingId = Guid.NewGuid();
    protected IOverclocking? _overclocking = null;

    public PresetBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public PresetBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public PresetBuilder WithDeviceName(string deviceName)
    {
        _deviceName = deviceName;
        return this;
    }

    public PresetBuilder WithOwnerId(Guid ownerId)
    {
        _ownerId = ownerId;
        return this;
    }

    public PresetBuilder WithVisible(bool isVisible = true)
    {
        _isVisible = isVisible;
        return this;
    }

    public PresetBuilder WithOverclocking(Func<IOverclocking> factory)
    {
        _overclocking = factory();
        _overclockingId = _overclocking.Id;
        return this;
    }

    public Preset Build()
    {
        return new Preset
        {
            Id = _id,
            Name = _name,
            DeviceName = _deviceName,
            OwnerId = _ownerId,
            IsVisible = _isVisible,
            OverclockingId = _overclockingId,
            Overclocking = _overclocking,
        };
    }
}
