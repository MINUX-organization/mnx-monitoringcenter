using AutoMapper;
using MNX.MonitoringCenter.Monitoring.Core;
using MNX.MonitoringCenter.Monitoring.UseCases.Commands.Subscription.SubscribeClient.Models;

namespace MNX.MonitoringCenter.Monitoring.UseCases;

/// <summary>
/// Конфигурация автомаппера.
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Rig, RigInformationMessage>();
    }
}
