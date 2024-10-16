using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MNX.MonitoringCenter.Inventory.Contracts;
using MNX.MonitoringCenter.Inventory.Contracts.Devices;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Cpu;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Drive;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Information;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Restrictions;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Motherboard;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.NetworkAdapter;
using MNX.MonitoringCenter.Inventory.Contracts.Requests;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rig;
using MNX.MonitoringCenter.Inventory.Contracts.RigInventory;
using MNX.MonitoringCenter.Inventory.UseCases;
using MNX.MonitoringCenter.Inventory.UseCases.Devices.Cpu;
using MNX.MonitoringCenter.Inventory.UseCases.Devices.Drive;
using MNX.MonitoringCenter.Inventory.UseCases.Devices.Gpu;
using MNX.MonitoringCenter.Inventory.UseCases.Devices.Motherboard;
using MNX.MonitoringCenter.Inventory.UseCases.Devices.NetworkAdapter;
using MNX.MonitoringCenter.Inventory.UseCases.Software;
using NUnit.Framework;

namespace MNX.MonitoringCenter.Inventory.IntegrationTests;

public class SaveInventoryTests : BaseTest
{
    private IMediator _mediator;

    private readonly static Guid OWNER_ID = Guid.Parse("1dcc87f7-a326-4765-befa-680ed6cb7649");

    [SetUp]
    public void SetUp()
    {
        _mediator = ServiceProvider.GetRequiredService<IMediator>();
    }

    [TestCaseSource(typeof(SaveCommandTestCase), nameof(SaveCommandTestCase.InventoryMessages))]
    public async Task SaveInventory(RigInventoryMsg inventoryMsg)
    {
        var rigRepository = ServiceProvider.GetRequiredService<IRigRepository>();
        await rigRepository.Add(new Rig()
        {
            Id = inventoryMsg.RigId,
            OwnerId = OWNER_ID,
            Name = "Minux"
        },
        default);

        var command = new SaveRigInventoryCommand(inventoryMsg);

        var result = await _mediator.Send(command);

        Assert.That(result.IsSuccess, Is.True, "Результат сохранения инвентаризации не успешен.");

        var cpuRepository = ServiceProvider.GetRequiredService<ICpuRepository>();
        var cpus = cpuRepository.GetCpus(new DeviceSpecification(OWNER_ID, inventoryMsg.RigId))
                                .ToBlockingEnumerable()
                                .ToList();

        var driveRepository = ServiceProvider.GetRequiredService<IDriveRepository>();
        var drives = await driveRepository
            .GetDrives(new InventorySpecification(OWNER_ID, inventoryMsg.RigId), default);
        

        var gpuRepository = ServiceProvider.GetRequiredService<IGpuRepository>();
        var gpus = gpuRepository.GetGpus(new DeviceSpecification(OWNER_ID, inventoryMsg.RigId))
                                .ToBlockingEnumerable()
                                .ToList();

        var motherboardRepository = ServiceProvider.GetRequiredService<IMotherboardRepository>();
        var motherboard = await motherboardRepository
            .GetMotherboardByRigId(inventoryMsg.RigId, OWNER_ID, default);

        var networkAdaptersRepository = ServiceProvider.GetRequiredService<INetworkAdapterRepository>();
        var adapters = await networkAdaptersRepository
            .GetNetworkAdapters(new InventorySpecification(OWNER_ID, inventoryMsg.RigId), default);

        var softwareRepository = ServiceProvider.GetRequiredService<ISoftwareRepository>();
        var software = await softwareRepository
            .GetSoftwareByRigId(inventoryMsg.RigId, OWNER_ID, default);

        Assert.Multiple(() =>
        {
            Assert.That(cpus, Is.EqualTo(inventoryMsg.Inventory.Cpus),
                        "Инвентаризация процессоров сохранилась неверно.");

            Assert.That(drives, Is.EqualTo(inventoryMsg.Inventory.Drives),
                        "Инвентаризация дисков сохранилась неверно.");

            Assert.That(gpus, Is.EqualTo(inventoryMsg.Inventory.Gpus),
                        "Инвентаризация видеокарт сохранилась неверно.");

            Assert.That(motherboard, Is.EqualTo(inventoryMsg.Inventory.Motherboard),
                        "Инвентаризация материнской платы сохранилась неверно.");

            Assert.That(adapters, Is.EqualTo(inventoryMsg.Inventory.NetworkAdapters),
                        "Инвентаризация сетевых адаптеров сохранилась неверно.");

            Assert.That(software, Is.EqualTo(inventoryMsg.Inventory.Software),
                        "Инвентаризация аппаратного обеспечения сохранилась неверно.");
        });
    }

    private class SaveCommandTestCase
    {
        public static IEnumerable<RigInventoryMsg> InventoryMessages
        {
            get
            {
                yield return new RigInventoryMsg()
                {
                    RigId = Guid.Parse("c220528c-a73b-4595-a4f9-21b2d03d503d"),
                    CreatedDateTime = DateTime.UtcNow,
                    Inventory = new RigInventoryModel()
                    {
                        Cpus = new()
                        {
                            new Cpu()
                            {
                                Id = Guid.NewGuid(),
                                Pci = new() { Id = 1, Bus = "00:1f.4" },
                                Information = new CpuInformation()
                                {
                                    Manufacturer = "Amd",
                                    Model = "Model",
                                    CoresCount = 10,
                                    ThreadsCount = 12,
                                    Architecture = "x86_64",
                                    Cache = new CpuCache() { L1 = 1, L2 = 2, L3 = 3 }
                                },
                                Restrictions = new CpuRestrictions()
                                {
                                    Power = new RangeValue(),
                                    FanSpeed = new RangeValue(),
                                    Temperature = new RangeValue(),
                                    Clock = new RangeValue()
                                }
                            }
                        },
                        Drives = new()
                        {
                            new Drive()
                            {
                                Id = Guid.NewGuid(),
                                Information = new DriveInformation()
                                {
                                    Manufacturer = "Manufacturer",
                                    Model = "Model",
                                    SerialNumber = "SerialNumber",
                                    Capacity = 1_000_000
                                }
                            }
                        },
                        Gpus = new()
                        {
                            new Gpu()
                            {
                                Id = Guid.NewGuid(),
                                Pci = new Pci() { Id = 1, Bus = "00:1f.4" },
                                Information = new GpuInformation()
                                {
                                    Manufacturer = "Amd",
                                    Model = "Model",
                                    SerialNumber = "SerialNumber",
                                    Vendor = "Vendor",
                                    BiosVersion = "1.0.0",
                                    Technology = new ParallelComputingTechnology()
                                    {
                                        Type = ParallelComputingTechnologyEnum.OpenCL,
                                        Version = "1.0.0"
                                    },
                                    Memory = new MemoryInformation()
                                    {
                                        Total = 1000,
                                        Type = "Type",
                                        Vendor = "Vendor"
                                    }
                                },
                                Restrictions = new GpuRestrictions()
                                {
                                    Power = new RangeValue(),
                                    FanSpeed = new RangeValue(),
                                    Temperature = new GpuTemperature()
                                    {
                                        Core = new RangeValue(),
                                        Memory = new RangeValue(),
                                    },
                                    Voltage = new GpuVoltage()
                                    {
                                        Core = new GpuChangingValue()
                                        {
                                            Lock = new RangeValue(),
                                            Offset = new RangeValue(),
                                        },
                                        Memory = new GpuChangingValue()
                                        {
                                            Lock = new RangeValue(),
                                            Offset = new RangeValue(),
                                        }
                                    },
                                    Clock = new GpuClock()
                                    {
                                        Core = new GpuChangingValue()
                                        {
                                            Lock = new RangeValue(),
                                            Offset = new RangeValue(),
                                        },
                                        Memory = new GpuChangingValue()
                                        {
                                            Lock = new RangeValue(),
                                            Offset = new RangeValue(),
                                        }
                                    }
                                }
                            }
                        },
                        NetworkAdapters = new()
                        {
                            new NetworkAdapter()
                            {
                                Id = Guid.NewGuid(),
                                GlobalIP = "192.168.0.1",
                                LocalIP = "127.0.0.1",
                                Information = new NetworkAdapterInformation()
                                {
                                    Manufacturer = "Manufacturer",
                                    Model = "Model",
                                    SerialNumber = "SerialNumber",
                                    VendorCode = "Vendor",
                                    BusInfo = "BusInfo",
                                    LogicalName = "LogicalName",
                                    Mac = "Mac address"
                                }
                            }
                        },
                        Motherboard = new Motherboard()
                        {
                            Id = Guid.NewGuid(),
                            Information = new MotherboardInformation()
                            {
                                Manufacturer = "Manufacturer",
                                Model = "Model",
                                SerialNumber = "SerialNumber"
                            },
                            Pcies = new()
                            {
                                new MotherboardPci() { Id = 1, Bus = "00:1f.4", IsInstalled = true },
                                new MotherboardPci() { Id = 2, Bus = "00:1f.5", IsInstalled = false }
                            }
                        },
                        Software = new SoftwareInventory()
                        {
                            Id = Guid.NewGuid(),
                            MinuxVersion = "1.0.0",
                            LinuxVersion = "1.0.0",
                            AmdDriverVersion = "1.0.0",
                            NvidiaDriverVersion = "1.0.0",
                            IntelDriverVersion = "1.0.0",
                            OpenCLVersion = "1.0.0",
                            CudaVersion = "1.0.0",
                            AgentVersion = "1.0.0",
                            HardwareManagerVersion = "1.0.0",
                            Miners = new()
                            {
                                { "miner_1", "1.0.0" },
                                { "miner_2", "1.0.0" }
                            }
                        }
                    }
                };

                yield return new RigInventoryMsg()
                {
                    RigId = Guid.Parse("b5bdffc5-338f-4719-9143-0c7ae9edb34b"),
                    CreatedDateTime = DateTime.UtcNow,
                    Inventory = new RigInventoryModel()
                    {
                        Cpus = new()
                        {
                            new Cpu()
                            {
                                Id = Guid.NewGuid(),
                                Pci = new() { Id = 1, Bus = "00:1f.4" },
                                Information = new CpuInformation()
                                {
                                    Manufacturer = "Amd",
                                    Model = "Model",
                                    CoresCount = 10,
                                    ThreadsCount = 12,
                                    Architecture = "x86_64",
                                    Cache = new CpuCache() { L1 = 1, L2 = 2, L3 = 3 }
                                },
                                Restrictions = new CpuRestrictions()
                                {
                                    Power = new RangeValue(),
                                    FanSpeed = new RangeValue(),
                                    Temperature = new RangeValue(),
                                    Clock = new RangeValue()
                                }
                            }
                        },
                        Drives = new()
                        {
                            new Drive()
                            {
                                Id = Guid.NewGuid(),
                                Information = new DriveInformation()
                                {
                                    Manufacturer = "Manufacturer",
                                    Model = "Model",
                                    SerialNumber = "SerialNumber",
                                    Capacity = 1_000_000
                                }
                            }
                        },
                        Gpus = new()
                        {
                            new Gpu()
                            {
                                Id = Guid.NewGuid(),
                                Pci = new Pci() { Id = 1, Bus = "00:1f.4" },
                                Information = new GpuInformation()
                                {
                                    Manufacturer = "Amd",
                                    Model = "Model",
                                    SerialNumber = "SerialNumber",
                                    Vendor = "Vendor",
                                    BiosVersion = "1.0.0",
                                    Technology = new ParallelComputingTechnology()
                                    {
                                        Type = ParallelComputingTechnologyEnum.OpenCL,
                                        Version = "1.0.0"
                                    },
                                    Memory = new MemoryInformation()
                                    {
                                        Total = 1000,
                                        Type = "Type",
                                        Vendor = "Vendor"
                                    }
                                },
                                Restrictions = new GpuRestrictions()
                                {
                                    Power = new RangeValue(),
                                    FanSpeed = new RangeValue(),
                                    Temperature = new GpuTemperature()
                                    {
                                        Core = new RangeValue(),
                                        Memory = new RangeValue(),
                                    },
                                    Voltage = new GpuVoltage()
                                    {
                                        Core = new GpuChangingValue()
                                        {
                                            Lock = new RangeValue(),
                                            Offset = new RangeValue(),
                                        },
                                        Memory = new GpuChangingValue()
                                        {
                                            Lock = new RangeValue(),
                                            Offset = new RangeValue(),
                                        }
                                    },
                                    Clock = new GpuClock()
                                    {
                                        Core = new GpuChangingValue()
                                        {
                                            Lock = new RangeValue(),
                                            Offset = new RangeValue(),
                                        },
                                        Memory = new GpuChangingValue()
                                        {
                                            Lock = new RangeValue(),
                                            Offset = new RangeValue(),
                                        }
                                    }
                                }
                            }
                        },
                        NetworkAdapters = new()
                        {
                            new NetworkAdapter()
                            {
                                Id = Guid.NewGuid(),
                                GlobalIP = "192.168.0.1",
                                LocalIP = "127.0.0.1",
                                Information = new NetworkAdapterInformation()
                                {
                                    Manufacturer = "Manufacturer",
                                    Model = "Model",
                                    SerialNumber = "SerialNumber",
                                    VendorCode = "Vendor",
                                    BusInfo = "BusInfo",
                                    LogicalName = "LogicalName",
                                    Mac = "Mac address"
                                }
                            }
                        },
                        Motherboard = new Motherboard()
                        {
                            Id = Guid.NewGuid(),
                            Information = new MotherboardInformation()
                            {
                                Manufacturer = "Manufacturer",
                                Model = "Model",
                                SerialNumber = "SerialNumber"
                            },
                            Pcies = new()
                            {
                                new MotherboardPci() { Id = 1, Bus = "00:1f.4", IsInstalled = true },
                                new MotherboardPci() { Id = 2, Bus = "00:1f.5", IsInstalled = false }
                            }
                        },
                        Software = new SoftwareInventory()
                        {
                            Id = Guid.NewGuid(),
                            MinuxVersion = "1.0.0",
                            LinuxVersion = "1.0.0",
                            AmdDriverVersion = "1.0.0",
                            NvidiaDriverVersion = "1.0.0",
                            IntelDriverVersion = "1.0.0",
                            OpenCLVersion = "1.0.0",
                            CudaVersion = "1.0.0",
                            AgentVersion = "1.0.0",
                            HardwareManagerVersion = "1.0.0",
                            Miners = new()
                            {
                                { "miner_1", "1.0.0" },
                                { "miner_2", "1.0.0" }
                            }
                        }
                    }
                };
            }
        }
    }
}
