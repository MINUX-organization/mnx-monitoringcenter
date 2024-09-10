using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MNX.MonitoringCenter.Inventory.Contracts;
using MNX.MonitoringCenter.Inventory.Contracts.Cpu;
using MNX.MonitoringCenter.Inventory.Contracts.Drive;
using MNX.MonitoringCenter.Inventory.Contracts.Gpu;
using MNX.MonitoringCenter.Inventory.Contracts.Gpu.Information;
using MNX.MonitoringCenter.Inventory.Contracts.Gpu.Restrictions;
using MNX.MonitoringCenter.Inventory.Contracts.Motherboard;
using MNX.MonitoringCenter.Inventory.Contracts.NetworkAdapter;
using MNX.MonitoringCenter.Inventory.UseCases;
using MNX.MonitoringCenter.Inventory.UseCases.Cpu;
using MNX.MonitoringCenter.Inventory.UseCases.Drive;
using MNX.MonitoringCenter.Inventory.UseCases.Gpu;
using MNX.MonitoringCenter.Inventory.UseCases.Motherboard;
using MNX.MonitoringCenter.Inventory.UseCases.NetworkAdapter;
using MNX.MonitoringCenter.Inventory.UseCases.Software;
using NUnit.Framework;

namespace MNX.MonitoringCenter.Inventory.IntegrationTests;

public class SaveInventoryTests : BaseTest
{
    private IMediator _mediator;

    [SetUp]
    public void SetUp()
    {
        _mediator = ServiceProvider.GetRequiredService<IMediator>();
    }

    [TestCaseSource(typeof(SaveCommandTestCase), nameof(SaveCommandTestCase.InventoryMessages))]
    public async Task SaveInventory(InventoryMsg inventoryMsg)
    {
        var command = new SaveInventoryCommand(inventoryMsg);

        var result = await _mediator.Send(command);

        Assert.That(result.IsSuccess, Is.True, "Результат сохранения инвентаризации не успешен.");

        var cpuRepository = ServiceProvider.GetRequiredService<ICpuRepository>();
        var cpus = cpuRepository.GetList(new DeviceSpecification(inventoryMsg.RigOwnerId))
                                .ToBlockingEnumerable()
                                .ToList();

        var driveRepository = ServiceProvider.GetRequiredService<IDriveRepository>();
        var drives = await driveRepository
            .GetList(new InventorySpecification(inventoryMsg.RigOwnerId), default);
        

        var gpuRepository = ServiceProvider.GetRequiredService<IGpuRepository>();
        var gpus = gpuRepository.GetList(new DeviceSpecification(inventoryMsg.RigOwnerId))
                                .ToBlockingEnumerable()
                                .ToList();

        var motherboardRepository = ServiceProvider.GetRequiredService<IMotherboardRepository>();
        var motherboard = await motherboardRepository
            .GetByRigId(inventoryMsg.RigId, inventoryMsg.RigOwnerId, default);

        var networkAdaptersRepository = ServiceProvider.GetRequiredService<INetworkAdapterRepository>();
        var adapters = await networkAdaptersRepository
            .GetList(new InventorySpecification(inventoryMsg.RigOwnerId), default);

        var softwareRepository = ServiceProvider.GetRequiredService<ISoftwareRepository>();
        var software = await softwareRepository
            .GetByRigId(inventoryMsg.RigId, inventoryMsg.RigOwnerId, default);

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
        public static IEnumerable<InventoryMsg> InventoryMessages
        {
            get
            {
                yield return new InventoryMsg()
                {
                    RigOwnerId = Guid.Parse("0b8e36f9-bf02-4c88-97f8-cb5a81715000"),
                    RigId = Guid.NewGuid(),
                    CreatedDateTime = DateTime.UtcNow,
                    Inventory = new InventoryModel()
                    {
                        Cpus = new()
                        {
                            new Contracts.Cpu.Cpu()
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
                            new Contracts.Drive.Drive()
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
                            new Contracts.Gpu.Gpu()
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
                        Motherboard = new Contracts.Motherboard.Motherboard()
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
