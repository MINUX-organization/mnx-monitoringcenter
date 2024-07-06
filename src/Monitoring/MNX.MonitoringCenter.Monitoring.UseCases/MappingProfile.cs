using AutoMapper;
using MNX.MonitoringCenter.Monitoring.Contracts.Bus.Models;
using MNX.MonitoringCenter.Monitoring.Core;
using MNX.MonitoringCenter.Monitoring.Core.Devices.Abstractions;
using MNX.MonitoringCenter.Monitoring.Core.Devices.Cpu;
using MNX.MonitoringCenter.Monitoring.Core.Devices.Gpu;
using MNX.MonitoringCenter.Monitoring.UseCases.Queries.Devices.Cpu.GetCpusInfo;
using MNX.MonitoringCenter.Monitoring.UseCases.Queries.Devices.Gpu.GetGpusInfo;
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
            .ForMember(message => message.TotalGpusCount, member => member.MapFrom(rig => rig.TotalGpusCount.Total))

            .ForMember(message => message.Devices, member => member.MapFrom(rig => rig.Devices.Where(device => device.FlightSheet != null)));

        CreateMap<MiningDevice, MiningDeviceModel>();

        CreateMap<FlightSheet, FlightSheetModel>();

        CreateMap<FlightSheetCoin, CoinModel>()
            .ForMember(model => model.FullName, member => member.MapFrom(coin => coin.Coin.FullName))
            .ForMember(model => model.ShortName, member => member.MapFrom(coin => coin.Coin.ShortName));

        CreateMap<GpuOverclocking, GpuOverclockingModel>().ReverseMap();

        CreateMap<Gpu, GpuInfo>()
            .ConstructUsing(gpu => new GpuInfo()
            {
                Id = gpu.Id,
                Name = gpu.Name,
                RigName = gpu.RigName,
                Manufacturer = gpu.Manufacturer,
                Miner = gpu.Miner,
                PciBusId = gpu.PciBusId,
                CriticalTemperature = gpu.CriticalTemperature,
                PowerLimit = gpu.PowerLimit,
                Technology = gpu.Technology,
                Vendor = gpu.Vendor,
                MemorySize = gpu.MemorySize,
                MemoryVendor = gpu.MemoryVendor,
                MemoryType = gpu.MemoryType,
                BiosVersion = gpu.BiosVersion,
                DriverVersion = gpu.DriverVersion
            })
            .AfterMap((gpu, gpuInfo) => gpuInfo.FlightSheet = gpu.FlightSheet?.Name);

        CreateMap<Cpu, CpuInfo>()
            .ConstructUsing(cpu => new CpuInfo()
            {
                Id = cpu.Id,
                Name = cpu.Name,
                RigName = cpu.RigName,
                Manufacturer = cpu.Manufacturer,
                FlightSheet = cpu.FlightSheet == null ? null : cpu.FlightSheet.Name,
                Miner = cpu.Miner,
                Architecture = cpu.Architecture,
                CoresCount = cpu.CoresCount,
                ThreadsCount = cpu.ThreadsCount,
                ThreadsPerSocketCount = cpu.ThreadsCount,
                MinClock = cpu.MinClock,
                MaxClock = cpu.MaxClock
            })
            .AfterMap((cpu, cpuInfo) => cpuInfo.FlightSheet = cpu.FlightSheet?.Name);
    }
}
