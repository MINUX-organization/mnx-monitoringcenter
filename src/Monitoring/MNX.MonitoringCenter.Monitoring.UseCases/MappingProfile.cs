using AutoMapper;
using MNX.MonitoringCenter.Monitoring.Contracts.Bus.Models;
using MNX.MonitoringCenter.Monitoring.Core.Devices.Gpu;

namespace MNX.MonitoringCenter.Monitoring.UseCases;

/// <summary>
/// Конфигурация автомаппера.
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<GpuOverclocking, GpuOverclockingModel>().ReverseMap();
    }
}
