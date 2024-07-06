using AutoMapper;
using MNX.MonitoringCenter.Monitoring.Core;
using MNX.MonitoringCenter.Monitoring.Core.Devices;
using MNX.MonitoringCenter.Monitoring.Core.Devices.Abstractions;
using MNX.MonitoringCenter.Monitoring.Core.Devices.Cpu;
using MNX.MonitoringCenter.Monitoring.Core.Devices.Enums;
using MNX.MonitoringCenter.Monitoring.Core.Devices.Gpu;
using MNX.MonitoringCenter.Monitoring.Core.Extensions;
using MNX.MonitoringCenter.Monitoring.DataAccess.Dto;
using MNX.MonitoringCenter.Monitoring.DataAccess.Dto.Devices;
using MNX.MonitoringCenter.Monitoring.DataAccess.Dto.Devices.Gpu;

namespace MNX.MonitoringCenter.Monitoring.DataAccess;

/// <summary>
/// Конфигурация маппера для БД.
/// </summary>
public class DbMappingProfile : Profile
{
    public DbMappingProfile()
    {
        CreateMap<RigDto, Rig>()
            .ConstructUsing((dto, context) => new Rig()
            {
                Devices = context.Mapper.Map<List<MiningDevice>>(dto.Devices)
            })
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
            }));

        // flight sheets

        CreateMap<FlightSheetDto, FlightSheet>().ReverseMap();

        CreateMap<FlightSheetCoinDto, FlightSheetCoin>().ReverseMap();

        // overclocking

        CreateMap<GpuOverclockingDto, GpuOverclocking>();

        CreateMap<GpuOverclocking, GpuOverclockingDto>()
            .ConstructUsing(dto => new GpuOverclockingDto
            {
                Id = Guid.NewGuid(),
                CoreClockLock = dto.CoreClockLock,
                CoreClockOffset = dto.CoreClockOffset,
                MemoryClockLock = dto.MemoryClockLock,
                MemoryClockOffset = dto.MemoryClockOffset,
                CoreVoltage = dto.CoreVoltage,
                CoreVoltageOffset = dto.CoreVoltageOffset,
                MemoryVoltage = dto.MemoryVoltage,
                MemoryVoltageOffset = dto.MemoryVoltageOffset,
                PowerLimit = dto.PowerLimit,
                CriticalTemperature = dto.CriticalTemperature,
                FanSpeed = dto.FanSpeed
            });

        // devices

        CreateMap<MiningDeviceDto, MiningDevice>().ConvertUsing(new MiningDeviceConverter());

        CreateMap<CpuDto, Cpu>()
            .ConstructUsing(dto => new Cpu(dto.Id,
                                           dto.Name,
                                           dto.Rig!.Name,
                                           dto.Manufacturer.ToEnum<CpuManufacturerEnum>(),
                                           dto.SerialNumber,
                                           dto.Architecture,
                                           dto.CoresCount,
                                           dto.ThreadsCount,
                                           dto.ThreadsPerSocketCount,
                                           dto.MinClock,
                                           dto.MaxClock)
            {
                Miner = dto.Miner
            })
            .ForMember(cpu => cpu.FlightSheet, member => member.MapFrom(dto => dto.FlightSheet));

        CreateMap<GpuDto, Gpu>()
            .ConstructUsing(dto => new Gpu(dto.Id,
                                           dto.Name,
                                           dto.Rig!.Name,
                                           dto.Manufacturer.ToEnum<GpuManufacturerEnum>(),
                                           dto.SerialNumber,
                                           dto.PciBusId,
                                           dto.CriticalTemperature,
                                           dto.PowerLimit,
                                           new ParallelComputingTechnology() { Type = dto.Technology, Version = dto.TechnologyVersion },
                                           dto.Vendor,
                                           dto.MemorySize,
                                           dto.MemoryVendor,
                                           dto.MemoryType,
                                           dto.BiosVersion,
                                           dto.Manufacturer.ToString() == GpuManufacturerEnum.Nvidia.ToString()
                                                                    ? dto.Rig.NvidiaDriverVersion
                                                                    :
                                                          dto.Manufacturer.ToString() == GpuManufacturerEnum.Amd.ToString()
                                                                    ? dto.Rig.AmdDriverVersion
                                                                    : dto.Rig.IntelDriverVersion)
            {
                Miner = dto.Miner
            })
            .ForMember(gpu => gpu.FlightSheet, member => member.MapFrom(dto => dto.FlightSheet));

        CreateMap<HddDto, Hdd>()
            .ConstructUsing(dto => new Hdd(dto.Id,
                                           dto.Name,
                                           dto.Rig!.Name,
                                           dto.Manufacturer,
                                           dto.SerialNumber,
                                           dto.Capacity)
            {
                Miner = dto.Miner
            })
            .ForMember(hdd => hdd.FlightSheet, member => member.MapFrom(dto => dto.FlightSheet));
    }

    /// <summary>
    /// Конвертер майнинг устройства.
    /// </summary>
    private class MiningDeviceConverter : ITypeConverter<MiningDeviceDto, MiningDevice>
    {
        public MiningDevice Convert(MiningDeviceDto source, MiningDevice destination, ResolutionContext context)
        {
            if (source is CpuDto cpuDto)
            {
                return context.Mapper.Map<Cpu>(cpuDto);
            }
            else if (source is GpuDto gpuDto)
            {
                return context.Mapper.Map<Gpu>(gpuDto);
            }
            else if (source is HddDto hddDto)
            {
                return context.Mapper.Map<Hdd>(hddDto);
            }

            return default!;
        }
    }
}
