using AutoMapper;
using MNX.MonitoringCenter.Management.Contracts.MiningDevice;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet;
using MNX.MonitoringCenter.Management.Agent.Commands.Mining.ApplySettings.Models;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping;

/// <summary>
/// Профиль маппинга для сущностей, связанных с сущностью <see cref="Miner"/>.
/// </summary>
public class MiningDeviceMappingProfile : Profile
{
    public MiningDeviceMappingProfile()
    {
        CreateMap<MiningDeviceInfo, MiningDeviceModel>().ConstructUsing(info => new MiningDeviceModel()
        {
            Id = info.Id,
            Manufacturer = info.Manufacturer,
            Model = info.Model,
            RigId = info.RigId!.Value,
            Type = info.Type.ToString(),
            FlightSheetId = info.FlightSheetId,
            FlightSheetName = info.FlightSheet != null ? info.FlightSheet.Name : null,
            FlightSheetIsConfirm = info.FlightSheetIsConfirm,
            MinerName = info.FlightSheet != null
                            ? info.FlightSheet.Targets.First(x => x.DeviceType == info.Type).Miner!.Name
                            : null
        });

        CreateMap<DeviceFLightSheet, WorkerSettings>().ConstructUsing((x, c) => new WorkerSettings()
        {
            WorkerId = x.Device.Id,
            SettingsModel = c.Mapper.Map<BaseMiningSettingsModel>(x.FlightSheet)
        });
    }
}
