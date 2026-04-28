using MNX.MonitoringCenter.Inventory.Contracts;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Overclocking;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Restrictions;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Motherboard;

namespace MNX.MonitoringCenter.RigsApi.Service.Tests.RigInventoryMsgConsumer;

internal static class RigInventoryTestCaseSources
{
    public static IEnumerable<RigInventoryMsg> ValidRigInventoryMessages
    {
        get
        {
            yield return new RigInventoryMsg
            {
                RigId = Guid.NewGuid(),
                RigOwnerId = Guid.NewGuid(),
                CreatedDateTime = DateTimeOffset.UtcNow,
                Inventory = new RigInventoryModel
                {
                    Cpus =
                [
                    new()
                    {
                        Information = new()
                        {
                            Manufacturer = "ManufacturerCPU",
                            Model = "ModelCPU",
                            Architecture = "x86_64",
                            Cache = new()
                            {
                                L1 = 1,
                            },
                            CoresCount = 1,
                            ThreadsCount = 1
                        },
                        Overclocking = new()
                        {
                            CoreClockLock = 0,
                            CoreVoltage = 0
                        },
                        Pci = new()
                        {
                            Bus = "00:00.0"
                        },
                        Restrictions = new()
                        {
                            Clock = new(),
                            FanSpeed = new(),
                            Power = new(),
                            Temperature = new()
                        }
                    }
                ],
                    Gpus =
                [
                    new()
                    {
                        Information = new()
                        {
                            Manufacturer = "X",
                            Model = "X",
                            Memory = new()
                            {
                                Total = 1
                            },
                            Technology = new()
                        },
                        Overclocking = new AmdGpuOverclocking()
                        {
                            FanSpeed = 100,
                            PowerLimit = 100
                        },
                        Pci = new()
                        {
                            Bus = "00:00.1"
                        },
                        Restrictions = new AmdGpuRestrictions()
                        {
                            FanSpeed = new(),
                            Power = new(),
                            TemperatureCore = new(),
                            TemperatureMemory = new(),
                            ClockCoreLock = new(),
                            ClockMemoryLock = new(),
                            VoltageCoreLock = new(),
                            VoltageCoreOffset = new(),
                            VoltageMemoryLock = new(),
                            ClockCoreState = new(),
                            ClockMemoryState = new(),
                            SocFrequency = new(),
                            SocVoltage = new(),
                            VoltageMemoryController = new()
                        }
                    }
                ],
                    Motherboard = new()
                    {
                        Information = new()
                        {
                            Manufacturer = "M",
                            Model = "X"
                        },
                        Pcies =
                    [
                        new MotherboardPci()
                        {
                            Bus = "00:00.0",
                            IsInstalled = true,
                        },
                        new MotherboardPci()
                        {
                            Bus = "00:00.1",
                            IsInstalled = true,
                        },
                    ]
                    },
                    Software = new()
                    {
                        AgentVersion = "1.0.1",
                        Miners =
                    [
                        new()
                        {
                            Name = "X",
                            Version = "X"
                        }
                    ]
                    },
                    NetworkAdapters =
                [
                    new()
                    {
                        Information = new()
                        {
                            LogicalName = "X",
                            Mac = "X"
                        },
                        GlobalIP = "192.168.0.1",
                        LocalIP = "192.168.0.2"
                    }
                ],
                    Drives =
                [
                    new()
                    {
                        Information = new()
                        {
                            Manufacturer = "X",
                            Model = "X"
                        }
                    }
                ]
                }
            };
        }
    }

    public static IEnumerable<RigInventoryMsg> InvalidRigInventoryMessages
    {
        get
        {
            yield return new RigInventoryMsg
            {
                RigId = Guid.NewGuid(),
                RigOwnerId = Guid.NewGuid(),
                CreatedDateTime = DateTimeOffset.UtcNow,
                Inventory = new RigInventoryModel
                {
                    Cpus =
                [
                    new()
                    {
                        Information = new()
                        {
                            Manufacturer = "ManufacturerCPU",
                            Model = "ModelCPU",
                            Architecture = "x86_64",
                            Cache = new()
                            {
                                L1 = 1,
                            }
                        },
                        Overclocking = new()
                        {

                        },
                        Pci = new()
                        {
                            Bus = "00:00.0"
                        },
                        Restrictions = new()
                        {
                            Clock = new(),
                            FanSpeed = new(),
                            Power = new(),
                            Temperature = new()
                        }
                    }
                ],
                    Motherboard = new()
                    {
                        Information = new()
                        {
                            Manufacturer = "M",
                            Model = "X"
                        },
                        Pcies =
                    [
                        new MotherboardPci()
                        {
                            Bus = "00:00.0",
                            IsInstalled = true,
                        }]
                    },
                    Software = new()
                    {
                        AgentVersion = "1.0.1"
                    }
                }
            };
        }
    }
}
