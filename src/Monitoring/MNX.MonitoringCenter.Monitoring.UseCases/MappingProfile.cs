using AutoMapper;
using MNX.MonitoringCenter.Monitoring.Contracts.Models;
using MNX.MonitoringCenter.Monitoring.Core;

namespace MNX.MonitoringCenter.Monitoring.UseCases;

/// <summary>
/// Конфигурация автомаппера.
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Rig, RigInformation>();
    }
}
