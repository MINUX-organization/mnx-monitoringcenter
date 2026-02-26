using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;

namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders.MiningDevices;

using MiningDevice = Core.Mining.MiningDevice.MiningDevice;

public class MiningDeviceBuilder<TBuilder>
    where TBuilder : MiningDeviceBuilder<TBuilder>
{
    protected static int _counter = 1;

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

    public virtual MiningDevice Build()
    {
        return new MiningDevice
        {
            Id = _id,
            Manufacturer = _manufacturer,
            Model = _model,
            OwnerId = _ownerId,
            Type = _type,
        };
    }
}
