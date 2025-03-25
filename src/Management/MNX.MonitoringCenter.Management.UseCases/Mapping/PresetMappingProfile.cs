using AutoMapper;
using MNX.MonitoringCenter.Management.Contracts.Presets;
using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.UseCases.Overclocking.Presets.Commands.EditPreset;
using MNX.MonitoringCenter.Management.UseCases.Overclocking.Presets.Commands.SavePreset;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping;

/// <summary>
/// Профиль маппинга для сущностей, связанных с сущностью <see cref="Preset"/>.
/// </summary>
public class PresetMappingProfile : Profile
{
    public PresetMappingProfile()
    {
        CreateMap<Preset, PresetModel>();

        CreateMap<EditPresetCommand, Preset>()
            .ForMember(destination => destination.Name, options => options.MapFrom(source => source.Model.Name))
            .ForMember(destination => destination.DeviceName, options => options.MapFrom(source => source.Model.DeviceName))
            .ForMember(destination => destination.Overclocking, options => options.MapFrom(source => source.Model.Overclocking))
            .AfterMap((command, preset) => preset.OverclockingId = preset.Overclocking!.Id);

        CreateMap<SavePresetCommand, Preset>()
            .ForMember(destination => destination.Name, options => options.MapFrom(source => source.Model.Name))
            .ForMember(destination => destination.DeviceName, options => options.MapFrom(source => source.Model.DeviceName))
            .ForMember(destination => destination.Overclocking, options => options.MapFrom(source => source.Model.Overclocking))
            .AfterMap((command, preset) => preset.OverclockingId = preset.Overclocking!.Id);
    }
}
