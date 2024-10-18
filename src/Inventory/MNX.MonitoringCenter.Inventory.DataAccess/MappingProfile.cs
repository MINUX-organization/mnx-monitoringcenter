using AutoMapper;
using MNX.MonitoringCenter.Inventory.Contracts;
using MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Software;

namespace MNX.MonitoringCenter.Inventory.DataAccess;

/// <summary>
/// Конфигурация автомапера.
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<SoftwareInventory, SoftwareInventoryDto>().ReverseMap();
    }
}
