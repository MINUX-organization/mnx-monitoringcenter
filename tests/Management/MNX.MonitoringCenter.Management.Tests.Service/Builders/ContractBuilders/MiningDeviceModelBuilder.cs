using MNX.MonitoringCenter.Management.Contracts.MiningDevice;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;

namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.ContractBuilders;

public class MiningDeviceModelBuilder
{
    private static int _counter = 1;

    protected Guid _id = Guid.NewGuid();
    protected string _manufacturer = $"MiningDeviceManufacturer_{_counter}";
    protected string _model = $"MiningDeviceModel_{_counter++}";
    protected string _type = MiningDeviceType.GPU.ToString();
    protected FlightSheetConfirmationState _flightSheetConfirmationState = FlightSheetConfirmationState.Unconfirmed;
    protected Guid? _flightSheetId = null;
    protected string? _flightSheetName = null;
    protected bool _isOnline = false;
    protected string? _minerName = null;
    protected string? _minerVersion = null;
    protected string? _presetName = null;
    protected Guid _rigId = Guid.NewGuid();

    public MiningDeviceModelBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public MiningDeviceModelBuilder WithManufacturer(string manufacturer)
    {
        _manufacturer = manufacturer;
        return this;
    }

    public MiningDeviceModelBuilder WithModel(string model)
    {
        _model = model;
        return this;
    }

    public MiningDeviceModelBuilder WithType(string type)
    {
        _type = type;
        return this;
    }

    public MiningDeviceModelBuilder WithFlightSheetConfirmationState(FlightSheetConfirmationState state)
    {
        _flightSheetConfirmationState = state;
        return this;
    }

    public MiningDeviceModelBuilder WithFlightSheetId(Guid? flightSheetId)
    {
        _flightSheetId = flightSheetId;
        return this;
    }

    public MiningDeviceModelBuilder WithFlightSheetName(string? flightSheetName)
    {
        _flightSheetName = flightSheetName;
        return this;
    }

    public MiningDeviceModelBuilder WithIsOnline(bool isOnline = true)
    {
        _isOnline = isOnline;
        return this;
    }

    public MiningDeviceModelBuilder WithMinerName(string? minerName)
    {
        _minerName = minerName;
        return this;
    }

    public MiningDeviceModelBuilder WithMinerVersion(string? minerVersion)
    {
        _minerVersion = minerVersion;
        return this;
    }

    public MiningDeviceModelBuilder WithPresetName(string? presetName)
    {
        _presetName = presetName;
        return this;
    }

    public MiningDeviceModelBuilder WithRigId(Guid rigId)
    {
        _rigId = rigId;
        return this;
    }

    public MiningDeviceModel Build()
    {
        return new MiningDeviceModel
        {
            Id = _id,
            Manufacturer = _manufacturer,
            Model = _model,
            Type = _type,
            FlightSheetConfirmationState = _flightSheetConfirmationState,
            FlightSheetId = _flightSheetId,
            FlightSheetName = _flightSheetName,
            IsOnline = _isOnline,
            MinerName = _minerName,
            MinerVersion = _minerVersion,
            PresetName = _presetName,
            RigId = _rigId,
        };
    }
}
