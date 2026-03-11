using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;

namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders.MiningDevices;

using FlightSheet = Core.Mining.FlightSheet.FlightSheet;
using Preset = Core.Overclocking.Preset;

public class MiningDeviceInfoBuilder : MiningDeviceBuilder<MiningDeviceInfoBuilder>
{
    protected Guid? _rigId = null;
    protected MiningDeviceLifeCycleStatus _lifeCycleStatus
        = MiningDeviceLifeCycleStatus.Offline;
    protected Guid? _presetId = null;
    protected Preset? _preset = null;
    protected FlightSheetConfirmationState _flightSheetConfirmationState
        = FlightSheetConfirmationState.Unconfirmed;
    protected Guid? _flightSheetId = null;
    protected FlightSheet? _flightSheet = null;

    public MiningDeviceInfoBuilder WithRig(Guid? rigId = null)
    {
        _rigId = rigId ?? Guid.NewGuid();
        return this;
    }

    public MiningDeviceInfoBuilder WithLifeCycleStatus(MiningDeviceLifeCycleStatus status)
    {
        _lifeCycleStatus = status;
        return this;
    }

    public MiningDeviceInfoBuilder WithConfirmationState(FlightSheetConfirmationState state)
    {
        _flightSheetConfirmationState = state;
        return this;
    }

    public MiningDeviceInfoBuilder WithPreset(Func<PresetBuilder, PresetBuilder>? configure = null)
    {
        var builder = new PresetBuilder();
        builder = configure?.Invoke(builder) ?? builder;
        _preset = builder.Build();
        _presetId = _preset.Id;
        return this;
    }

    public MiningDeviceInfoBuilder WithPreset(Preset preset)
    {
        _preset = preset;
        _presetId = preset.Id;
        return this;
    }

    public MiningDeviceInfoBuilder WithFlightSheet(Func<FlightSheetBuilder, FlightSheetBuilder>? configure = null)
    {
        var builder = new FlightSheetBuilder();
        builder = configure?.Invoke(builder) ?? builder;
        _flightSheet = builder.Build();
        _flightSheetId = _flightSheet.Id;
        return this;
    }

    public override MiningDeviceInfo Build()
    {
        return new MiningDeviceInfo
        {
            Id = _id,
            Manufacturer = _manufacturer,
            Model = _model,
            OwnerId = _ownerId,
            Type = _type,
            RigId = _rigId,
            LifeCycleStatus = _lifeCycleStatus,
            PresetId = _presetId,
            Preset = _preset,
            FlightSheetConfirmationState = _flightSheetConfirmationState,
            FlightSheetId = _flightSheetId,
            FlightSheet = _flightSheet,
        };
    }
}
