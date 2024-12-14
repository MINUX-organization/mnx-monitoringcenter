using EasyNetQ;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MNX.MonitoringCenter.Common.AgentMessages;
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
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs;
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

    private readonly static Guid OWNER_ID = Guid.Parse("0b8e36f9-bf02-4c88-97f8-cb5a81715000");

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
            .GetDrives(new DeviceSpecification(OWNER_ID, inventoryMsg.RigId), default);
        

        var gpuRepository = ServiceProvider.GetRequiredService<IGpuRepository>();
        var gpus = gpuRepository.GetGpus(new DeviceSpecification(OWNER_ID, inventoryMsg.RigId))
                                .ToBlockingEnumerable()
                                .ToList();

        var motherboardRepository = ServiceProvider.GetRequiredService<IMotherboardRepository>();
        var motherboard = await motherboardRepository
            .GetMotherboardByRigId(inventoryMsg.RigId, OWNER_ID, default);

        var networkAdaptersRepository = ServiceProvider.GetRequiredService<INetworkAdapterRepository>();
        var adapters = await networkAdaptersRepository
            .GetNetworkAdapters(new DeviceSpecification(OWNER_ID, inventoryMsg.RigId), default);

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

    [TestCaseSource(typeof(SaveCommandTestCase), nameof(SaveCommandTestCase.InventoryMessages))]
    public async Task SaveInventoryWithRabbitMq(RigInventoryMsg inventoryMsg)
    {
        using var bus = RabbitHutch.CreateBus("host=77.37.200.24:5672;username=guest;password=guest;publisherConfirms=true");
        await bus.PubSub.PublishAsync(new RigDisconnectedMsg(inventoryMsg.RigId));
        await bus.PubSub.PublishAsync(inventoryMsg);
    }

    private class SaveCommandTestCase
    {
        public static IEnumerable<RigInventoryMsg> InventoryMessages
        {
            get
            {
                var gpuRig1Id = Guid.Parse("10f81050-235f-464f-8724-c9cfcdd54551");
                var cpuRig1Id = Guid.Parse("10f81050-235f-464f-8724-c9cfcdd54552");
                var gpuRig2Id = Guid.Parse("10f81050-235f-464f-8724-c9cfcdd54533");
                var cpuRig2Id = Guid.Parse("10f81050-235f-464f-8724-c9cfcdd54544");

                yield return new RigInventoryMsg()
                {
                    RigId = Guid.Parse("10f81050-235f-464f-8724-c9cfcdd54558"),
                    RigOwnerId = OWNER_ID,
                    CreatedDateTime = DateTime.UtcNow,
                    Inventory = new RigInventoryModel()
                    {
                        Cpus = new()
                        {
                            new Cpu()
                            {
                                Id = cpuRig1Id,
                                Pci = new() { Id = 0, Bus = "00:00.0" },
                                Information = new CpuInformation()
                                {
                                    Manufacturer = "AMD",
                                    Model = "Ryzen 5 7500f",
                                    CoresCount = 10,
                                    ThreadsCount = 12,
                                    Architecture = "x86_64",
                                    Cache = new CpuCache() { L1 = 1, L2 = 2, L3 = 3 }
                                },
                                Restrictions = new CpuRestrictions()
                                {
                                    Power = new RangeValue()
                                    {
                                        Minimal = 30,
                                        Maximal = 150,
                                        Default = 100,
                                        IsWritable = false,
                                    },
                                    FanSpeed = new RangeValue()
                                    {
                                        Minimal = 0,
                                        Maximal = 100,
                                        Default = 50,
                                        IsWritable = false,
                                    },
                                    Temperature = new RangeValue()
                                    {
                                        Minimal = 30,
                                        Maximal = 120,
                                        Default = 100,
                                        IsWritable = false
                                    },
                                    Clock = new RangeValue()
                                    {
                                        Minimal = 2000,
                                        Maximal = 5000,
                                        Default = 2500,
                                        IsWritable = false
                                    }
                                },
                                Overclocking = new CpuOverclocking()
                                {
                                    CoreClockLock = 2500,
                                    CoreVoltage = 25
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
                                Id = gpuRig1Id,
                                Pci = new Pci() { Id = 1, Bus = "00:01.0" },
                                Information = new GpuInformation()
                                {
                                    Manufacturer = "AMD",
                                    Model = "RX 580",
                                    SerialNumber = "SerialNumber",
                                    Vendor = "MSI",
                                    BiosVersion = "1.0.0",
                                    Technology = new ParallelComputingTechnology()
                                    {
                                        Type = ParallelComputingTechnologyEnum.OpenCL,
                                        Version = "1.0.0"
                                    },
                                    Memory = new MemoryInformation()
                                    {
                                        Total = 1000,
                                        Type = "GDDR 4",
                                        Vendor = "Samsung"
                                    }
                                },
                                Restrictions = new GpuRestrictions()
                                {
                                    Power = new RangeValue()
                                    {
                                        Minimal = 30,
                                        Maximal = 150,
                                        Default = 100,
                                        IsWritable = true
                                    },
                                    FanSpeed = new RangeValue()
                                    {
                                        Minimal = 0,
                                        Maximal = 100,
                                        Default = 100,
                                        IsWritable = true
                                    },
                                    Temperature = new GpuTemperatureRestrictions()
                                    {
                                        Core = new RangeValue()
                                        {
                                            Minimal = 30,
                                            Maximal = 150,
                                            Default = 100,
                                            IsWritable = true
                                        },
                                        Memory = new RangeValue()
                                        {
                                            Minimal = 30,
                                            Maximal = 150,
                                            Default = 100,
                                            IsWritable = true
                                        },
                                    },
                                    Voltage = new GpuVoltageRestrictions()
                                    {
                                        Core = new GpuChangingValue()
                                        {
                                            Lock = new RangeValue() { Minimal = 30, Maximal = 75, Default = 50, IsWritable = true },
                                            Offset = new RangeValue() { Minimal = -30, Maximal = 50, Default = 0, IsWritable = true },
                                        },
                                        Memory = new GpuChangingValue()
                                        {
                                            Lock = new RangeValue() { Minimal = 30, Maximal = 75, Default = 50, IsWritable = true },
                                            Offset = new RangeValue(){ Minimal = -30, Maximal = 50, Default = 0, IsWritable = true },
                                        }
                                    },
                                    Clock = new GpuClockRestrictions()
                                    {
                                        Core = new GpuChangingValue()
                                        {
                                            Lock = new RangeValue() { Minimal = 300, Maximal = 2000, Default = 1500, IsWritable = true },
                                            Offset = new RangeValue() { Minimal = -1000, Maximal = 1000, Default = 0, IsWritable = true },
                                        },
                                        Memory = new GpuChangingValue()
                                        {
                                            Lock = new RangeValue(){ Minimal = 150, Maximal = 2500, Default = 1000, IsWritable = true },
                                            Offset = new RangeValue(){ Minimal = -30, Maximal = 75, Default = 50, IsWritable = true },
                                        }
                                    }
                                },
                                Overclocking = new GpuOverclocking()
                                {
                                    FanSpeed = 50,
                                    PowerLimit = 100,
                                    CoreClockLock = 1500,
                                    CoreClockOffset = 0,
                                    MemoryClockLock = 1000,
                                    MemoryClockOffset = 0,
                                    CoreVoltage = 50,
                                    CoreVoltageOffset = 0,
                                    MemoryVoltage = 50,
                                    MemoryVoltageOffset = 0
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
                                new MotherboardPci() { Id = 0, Bus = "00:00.0", IsInstalled = true },
                                new MotherboardPci() { Id = 1, Bus = "00:01.0", IsInstalled = true },
                                new MotherboardPci() { Id = 2, Bus = "00:02.0", IsInstalled = false },
                                new MotherboardPci() { Id = 3, Bus = "00:03.0", IsInstalled = false },
                                new MotherboardPci() { Id = 4, Bus = "00:04.0", IsInstalled = false },
                                new MotherboardPci() { Id = 5, Bus = "00:05.0", IsInstalled = false },
                                new MotherboardPci() { Id = 6, Bus = "00:06.0", IsInstalled = false },
                                new MotherboardPci() { Id = 7, Bus = "00:07.0", IsInstalled = false }
                            }
                        },
                        Software = new SoftwareInventory()
                        {
                            MinuxVersion = "1.0.0",
                            LinuxVersion = "1.0.0",
                            AmdGpuDriverVersion = "1.0.0",
                            NvidiaGpuDriverVersion = "1.0.0",
                            IntelGpuDriverVersion = "1.0.0",
                            OpenCLVersion = "1.0.0",
                            CudaVersion = "1.0.0",
                            AgentVersion = "1.0.0",
                            HardwareManagerVersion = "1.0.0",
                            Miners = new()
                            {
                                { "lolMiner", "1.0.0" },
                                { "rigel", "1.0.0" }
                            }
                        }
                    }
                };

                yield return new RigInventoryMsg()
                {
                    RigId = Guid.Parse("10f81050-235f-464f-8724-c9cfcdd54558"),
                    RigOwnerId = OWNER_ID,
                    CreatedDateTime = DateTime.UtcNow,
                    Inventory = new RigInventoryModel()
                    {
                        Cpus = new()
                        {
                            new Cpu()
                            {
                                Id = cpuRig1Id,
                                Pci = new() { Id = 0, Bus = "00:00.0" },
                                Information = new CpuInformation()
                                {
                                    Manufacturer = "AMD",
                                    Model = "Ryzen 5 7500f",
                                    CoresCount = 10,
                                    ThreadsCount = 12,
                                    Architecture = "x86_64",
                                    Cache = new CpuCache() { L1 = 1, L2 = 2, L3 = 3 }
                                },
                                Restrictions = new CpuRestrictions()
                                {
                                    Power = new RangeValue()
                                    {
                                        Minimal = 30,
                                        Maximal = 150,
                                        Default = 100,
                                        IsWritable = false,
                                    },
                                    FanSpeed = new RangeValue()
                                    {
                                        Minimal = 0,
                                        Maximal = 100,
                                        Default = 50,
                                        IsWritable = false,
                                    },
                                    Temperature = new RangeValue()
                                    {
                                        Minimal = 30,
                                        Maximal = 120,
                                        Default = 100,
                                        IsWritable = false
                                    },
                                    Clock = new RangeValue()
                                    {
                                        Minimal = 2000,
                                        Maximal = 5000,
                                        Default = 2500,
                                        IsWritable = false
                                    }
                                },
                                Overclocking = new CpuOverclocking()
                                {
                                    CoreClockLock = 2500,
                                    CoreVoltage = 25
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
                                Id = gpuRig1Id,
                                Pci = new Pci() { Id = 1, Bus = "00:01.0" },
                                Information = new GpuInformation()
                                {
                                    Manufacturer = "AMD",
                                    Model = "RX 580",
                                    SerialNumber = "SerialNumber",
                                    Vendor = "MSI",
                                    BiosVersion = "1.0.0",
                                    Technology = new ParallelComputingTechnology()
                                    {
                                        Type = ParallelComputingTechnologyEnum.OpenCL,
                                        Version = "1.0.0"
                                    },
                                    Memory = new MemoryInformation()
                                    {
                                        Total = 1000,
                                        Type = "GDDR 4",
                                        Vendor = "Samsung"
                                    }
                                },
                                Restrictions = new GpuRestrictions()
                                {
                                    Power = new RangeValue()
                                    {
                                        Minimal = 30,
                                        Maximal = 150,
                                        Default = 100,
                                        IsWritable = true
                                    },
                                    FanSpeed = new RangeValue()
                                    {
                                        Minimal = 0,
                                        Maximal = 100,
                                        Default = 100,
                                        IsWritable = true
                                    },
                                    Temperature = new GpuTemperatureRestrictions()
                                    {
                                        Core = new RangeValue()
                                        {
                                            Minimal = 30,
                                            Maximal = 150,
                                            Default = 100,
                                            IsWritable = true
                                        },
                                        Memory = new RangeValue()
                                        {
                                            Minimal = 30,
                                            Maximal = 150,
                                            Default = 100,
                                            IsWritable = true
                                        },
                                    },
                                    Voltage = new GpuVoltageRestrictions()
                                    {
                                        Core = new GpuChangingValue()
                                        {
                                            Lock = new RangeValue() { Minimal = 30, Maximal = 75, Default = 50, IsWritable = true },
                                            Offset = new RangeValue() { Minimal = -30, Maximal = 50, Default = 0, IsWritable = true },
                                        },
                                        Memory = new GpuChangingValue()
                                        {
                                            Lock = new RangeValue() { Minimal = 30, Maximal = 75, Default = 50, IsWritable = true },
                                            Offset = new RangeValue(){ Minimal = -30, Maximal = 50, Default = 0, IsWritable = true },
                                        }
                                    },
                                    Clock = new GpuClockRestrictions()
                                    {
                                        Core = new GpuChangingValue()
                                        {
                                            Lock = new RangeValue() { Minimal = 300, Maximal = 2000, Default = 1500, IsWritable = true },
                                            Offset = new RangeValue() { Minimal = -1000, Maximal = 1000, Default = 0, IsWritable = true },
                                        },
                                        Memory = new GpuChangingValue()
                                        {
                                            Lock = new RangeValue(){ Minimal = 150, Maximal = 2500, Default = 1000, IsWritable = true },
                                            Offset = new RangeValue(){ Minimal = -30, Maximal = 75, Default = 50, IsWritable = true },
                                        }
                                    }
                                },
                                Overclocking = new GpuOverclocking()
                                {
                                    FanSpeed = 70,
                                    PowerLimit = 100,
                                    CoreClockLock = 1500,
                                    CoreClockOffset = 0,
                                    MemoryClockLock = 1000,
                                    MemoryClockOffset = 0,
                                    CoreVoltage = 50,
                                    CoreVoltageOffset = 0,
                                    MemoryVoltage = 50,
                                    MemoryVoltageOffset = 0
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
                                new MotherboardPci() { Id = 0, Bus = "00:00.0", IsInstalled = true },
                                new MotherboardPci() { Id = 1, Bus = "00:01.0", IsInstalled = true },
                                new MotherboardPci() { Id = 2, Bus = "00:02.0", IsInstalled = false },
                                new MotherboardPci() { Id = 3, Bus = "00:03.0", IsInstalled = false },
                                new MotherboardPci() { Id = 4, Bus = "00:04.0", IsInstalled = false },
                                new MotherboardPci() { Id = 5, Bus = "00:05.0", IsInstalled = false },
                                new MotherboardPci() { Id = 6, Bus = "00:06.0", IsInstalled = false },
                                new MotherboardPci() { Id = 7, Bus = "00:07.0", IsInstalled = false }
                            }
                        },
                        Software = new SoftwareInventory()
                        {
                            MinuxVersion = "1.0.0",
                            LinuxVersion = "1.0.0",
                            AmdGpuDriverVersion = "1.0.0",
                            NvidiaGpuDriverVersion = "1.0.0",
                            IntelGpuDriverVersion = "1.0.0",
                            OpenCLVersion = "1.0.0",
                            CudaVersion = "1.0.0",
                            AgentVersion = "1.0.0",
                            HardwareManagerVersion = "1.0.0",
                            Miners = new()
                            {
                                { "lolMiner", "1.0.0" },
                                { "rigel", "1.0.0" }
                            }
                        }
                    }
                };

                yield return new RigInventoryMsg()
                {
                    RigId = Guid.Parse("58c5749c-84fc-4148-84ed-8532a195e733"),
                    RigOwnerId = OWNER_ID,
                    CreatedDateTime = DateTime.UtcNow,
                    Inventory = new RigInventoryModel()
                    {
                        Cpus = new()
                        {
                            new Cpu()
                            {
                                Id = cpuRig2Id,
                                Pci = new() { Id = 0, Bus = "00:00.0" },
                                Information = new CpuInformation()
                                {
                                    Manufacturer = "AMD",
                                    Model = "Ryzen 5 7500f",
                                    CoresCount = 10,
                                    ThreadsCount = 12,
                                    Architecture = "x86_64",
                                    Cache = new CpuCache() { L1 = 1, L2 = 2, L3 = 3 }
                                },
                                Restrictions = new CpuRestrictions()
                                {
                                    Power = new RangeValue()
                                    {
                                        Minimal = 30,
                                        Maximal = 150,
                                        Default = 100,
                                        IsWritable = false,
                                    },
                                    FanSpeed = new RangeValue()
                                    {
                                        Minimal = 0,
                                        Maximal = 100,
                                        Default = 50,
                                        IsWritable = false,
                                    },
                                    Temperature = new RangeValue()
                                    {
                                        Minimal = 30,
                                        Maximal = 120,
                                        Default = 100,
                                        IsWritable = false
                                    },
                                    Clock = new RangeValue()
                                    {
                                        Minimal = 2000,
                                        Maximal = 5000,
                                        Default = 2500,
                                        IsWritable = false
                                    }
                                },
                                Overclocking = new CpuOverclocking()
                                {
                                    CoreClockLock = 2500,
                                    CoreVoltage = 25
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
                                Id = gpuRig2Id,
                                Pci = new Pci() { Id = 1, Bus = "00:01.0" },
                                Information = new GpuInformation()
                                {
                                    Manufacturer = "Nvidia",
                                    Model = "GeForce RTX 8090",
                                    SerialNumber = "SerialNumber",
                                    Vendor = "MSI",
                                    BiosVersion = "1.0.0",
                                    Technology = new ParallelComputingTechnology()
                                    {
                                        Type = ParallelComputingTechnologyEnum.OpenCL,
                                        Version = "1.0.0"
                                    },
                                    Memory = new MemoryInformation()
                                    {
                                        Total = 1000,
                                        Type = "GDDR 4",
                                        Vendor = "Samsung"
                                    }
                                },
                                Restrictions = new GpuRestrictions()
                                {
                                    Power = new RangeValue()
                                    {
                                        Minimal = 30,
                                        Maximal = 150,
                                        Default = 100,
                                        IsWritable = true
                                    },
                                    FanSpeed = new RangeValue()
                                    {
                                        Minimal = 0,
                                        Maximal = 100,
                                        Default = 100,
                                        IsWritable = true
                                    },
                                    Temperature = new GpuTemperatureRestrictions()
                                    {
                                        Core = new RangeValue()
                                        {
                                            Minimal = 30,
                                            Maximal = 150,
                                            Default = 100,
                                            IsWritable = true
                                        },
                                        Memory = new RangeValue()
                                        {
                                            Minimal = 30,
                                            Maximal = 150,
                                            Default = 100,
                                            IsWritable = true
                                        },
                                    },
                                    Voltage = new GpuVoltageRestrictions()
                                    {
                                        Core = new GpuChangingValue()
                                        {
                                            Lock = new RangeValue() { Minimal = 30, Maximal = 75, Default = 50, IsWritable = true },
                                            Offset = new RangeValue() { Minimal = -30, Maximal = 50, Default = 0, IsWritable = true },
                                        },
                                        Memory = new GpuChangingValue()
                                        {
                                            Lock = new RangeValue() { Minimal = 30, Maximal = 75, Default = 50, IsWritable = true },
                                            Offset = new RangeValue(){ Minimal = -30, Maximal = 50, Default = 0, IsWritable = true },
                                        }
                                    },
                                    Clock = new GpuClockRestrictions()
                                    {
                                        Core = new GpuChangingValue()
                                        {
                                            Lock = new RangeValue() { Minimal = 300, Maximal = 2000, Default = 1500, IsWritable = true },
                                            Offset = new RangeValue() { Minimal = -1000, Maximal = 1000, Default = 0, IsWritable = true },
                                        },
                                        Memory = new GpuChangingValue()
                                        {
                                            Lock = new RangeValue(){ Minimal = 150, Maximal = 2500, Default = 1000, IsWritable = true },
                                            Offset = new RangeValue(){ Minimal = -30, Maximal = 75, Default = 50, IsWritable = true },
                                        }
                                    }
                                },
                                Overclocking = new GpuOverclocking()
                                {
                                    FanSpeed = 50,
                                    PowerLimit = 100,
                                    CoreClockLock = 1500,
                                    CoreClockOffset = 0,
                                    MemoryClockLock = 1000,
                                    MemoryClockOffset = 0,
                                    CoreVoltage = 50,
                                    CoreVoltageOffset = 0,
                                    MemoryVoltage = 50,
                                    MemoryVoltageOffset = 0
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
                                new MotherboardPci() { Id = 0, Bus = "00:00.0", IsInstalled = true },
                                new MotherboardPci() { Id = 1, Bus = "00:01.0", IsInstalled = true },
                                new MotherboardPci() { Id = 2, Bus = "00:02.0", IsInstalled = false },
                                new MotherboardPci() { Id = 3, Bus = "00:03.0", IsInstalled = false },
                                new MotherboardPci() { Id = 4, Bus = "00:04.0", IsInstalled = false },
                                new MotherboardPci() { Id = 5, Bus = "00:05.0", IsInstalled = false },
                                new MotherboardPci() { Id = 6, Bus = "00:06.0", IsInstalled = false },
                                new MotherboardPci() { Id = 7, Bus = "00:07.0", IsInstalled = false }
                            }
                        },
                        Software = new SoftwareInventory()
                        {
                            MinuxVersion = "1.0.0",
                            LinuxVersion = "1.0.0",
                            AmdGpuDriverVersion = "1.0.0",
                            NvidiaGpuDriverVersion = "1.0.0",
                            IntelGpuDriverVersion = "1.0.0",
                            OpenCLVersion = "1.0.0",
                            CudaVersion = "1.0.0",
                            AgentVersion = "1.0.0",
                            HardwareManagerVersion = "1.0.0",
                            Miners = new()
                            {
                                { "lolMiner", "1.0.0" },
                                { "rigel", "1.0.0" }
                            }
                        }
                    }
                };
            }
        }
    }
}
