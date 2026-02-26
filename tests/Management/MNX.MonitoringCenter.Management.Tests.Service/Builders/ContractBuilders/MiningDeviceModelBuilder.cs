using MNX.MonitoringCenter.Management.Contracts.MiningDevice;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;

namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.ContractBuilders;

public class MiningDeviceModelBuilder
{
    private static int _counter = 1;

    private Guid _id = Guid.NewGuid();
    private string _manufacturer = $"MiningDeviceManufacturer_{_counter}";
    private string _model = $"MiningDeviceModel_{_counter++}";
    private string _type = MiningDeviceType.GPU.ToString();
    private FlightSheetConfirmationState _flightSheetConfirmationState = FlightSheetConfirmationState.Unconfirmed;
    private Guid? _flightSheetId = null;
    private string? _flightSheetName = null;
    private bool _isOnline = false;
    private string? _minerName = null;
    private string? _minerVersion = null;
    private string? _presetName = null;
    private Guid _rigId = Guid.NewGuid();

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
