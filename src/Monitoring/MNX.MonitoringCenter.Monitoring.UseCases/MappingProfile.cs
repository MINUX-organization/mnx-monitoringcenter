using AutoMapper;
using MNX.MonitoringCenter.Monitoring.Core;
using MNX.MonitoringCenter.Monitoring.UseCases.Queries.GetRigsInformation;

namespace MNX.MonitoringCenter.Monitoring.UseCases;

/// <summary>
/// Конфигурация автомаппера.
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Rig, RigInformationMessage>()
            .ForMember(message => message.AmdCpusCount, member => member.MapFrom(rig => rig.TotalCpusCount.Amd))
            .ForMember(message => message.IntelCpusCount, member => member.MapFrom(rig => rig.TotalCpusCount.Intel))
            .ForMember(message => message.TotalCpusCount, member => member.MapFrom(rig => rig.TotalCpusCount.Total))

            .ForMember(message => message.AmdGpusCount, member => member.MapFrom(rig => rig.TotalGpusCount.Amd))
            .ForMember(message => message.NvidiaGpusCount, member => member.MapFrom(rig => rig.TotalGpusCount.Nvidia))
            .ForMember(message => message.IntelGpusCount, member => member.MapFrom(rig => rig.TotalGpusCount.Intel))
            .ForMember(message => message.TotalGpusCount, member => member.MapFrom(rig => rig.TotalGpusCount.Total));

        CreateMap<FlightSheet, FlightSheetModel>();

        CreateMap<Coin, CoinModel>();
    }
}
