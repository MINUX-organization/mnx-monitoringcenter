using MNX.MonitoringCenter.Management.Contracts.MiningDevice;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice;
using MNX.MonitoringCenter.Management.UseCases.Mining.MiningDevice;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.MiningDevice;

/// <summary>
/// Реализация <see cref="IMiningDeviceMapper"/>.
/// </summary>
public class MiningDeviceMapper : IMiningDeviceMapper
{
    /// <inheritdoc/>
    public MiningDeviceModel MapToModel(MiningDeviceInfo model)
    {
        var minerName = model.FlightSheet != null ?
            model.FlightSheet.Targets.First(x => x.DeviceType == model.Type).Miner!.Name : null;

        var minerVersion = model.FlightSheet != null ?
            model.FlightSheet.Targets.First(x => x.DeviceType == model.Type).Miner!.Version : null;

        return new MiningDeviceModel()
        {
            Id = model.Id,
            Manufacturer = model.Manufacturer,
            Model = model.Model,
            Type = model.Type.ToString(),
            RigId = model.RigId!.Value,
            FlightSheetId = model.FlightSheetId,
            FlightSheetName = model.FlightSheet?.Name,
            PresetName = model.Preset?.Name,
            FlightSheetConfirmationState = model.FlightSheetConfirmationState,
            MinerName = minerName,
            MinerVersion = minerVersion,
            IsOnline = model.IsOnline,
        };
    }
}
