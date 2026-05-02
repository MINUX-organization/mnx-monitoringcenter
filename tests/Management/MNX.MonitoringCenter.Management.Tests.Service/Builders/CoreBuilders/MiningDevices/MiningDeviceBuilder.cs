using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;

namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders.MiningDevices;

using MiningDevice = Core.Mining.MiningDevice.MiningDevice;

public abstract class MiningDeviceBuilder<TBuilder>
    where TBuilder : MiningDeviceBuilder<TBuilder>
{
    private static int _counter = 1;

    protected Guid _id = Guid.NewGuid();
    protected string _manufacturer = $"DeviceManufacturer_{_counter}";
    protected string _model = $"DeviceModel_{_counter++}";
    protected Guid? _ownerId = null;
    protected MiningDeviceType _type = MiningDeviceType.GPU;

    public TBuilder WithId(Guid id)
    {
        _id = id;
        return (TBuilder)this;
    }

    public TBuilder WithManufacturer(string manufacturer)
    {
        _manufacturer = manufacturer;
        return (TBuilder)this;
    }

    public TBuilder WithModel(string model)
    {
        _model = model;
        return (TBuilder)this;
    }

    public TBuilder WithOwner(Guid? ownerId = null)
    {
        _ownerId = ownerId;
        return (TBuilder)this;
    }

    public TBuilder WithDeviceType(MiningDeviceType type)
    {
        _type = type;
        return (TBuilder)this;
    }

    public abstract MiningDevice Build();
}
