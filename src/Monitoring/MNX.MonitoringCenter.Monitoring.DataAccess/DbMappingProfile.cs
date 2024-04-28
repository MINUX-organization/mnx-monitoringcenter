using AutoMapper;
using MNX.MonitoringCenter.Monitoring.Core;
using MNX.MonitoringCenter.Monitoring.DataAccess.Dto;

namespace MNX.MonitoringCenter.Monitoring.DataAccess;

/// <summary>
/// Конфигурация маппера для БД.
/// </summary>
public class DbMappingProfile : Profile
{
    public DbMappingProfile()
    {
        CreateMap<RigDto, Rig>()
            .ForMember(rig => rig.TotalGpusCount, member => member.MapFrom(dto => new TotalGpusCount()
            {
                Amd = dto.AmdGpusCount,
                Nvidia = dto.NvidiaGpusCount,
                Intel = dto.IntelGpusCount,
            }))
            .ForMember(rig => rig.TotalCpusCount, member => member.MapFrom(dto => new TotalCpusCount()
            {
                Amd = dto.AmdCpusCount,
                Intel = dto.IntelCpusCount,
            }))
            .ReverseMap();
    }
}
