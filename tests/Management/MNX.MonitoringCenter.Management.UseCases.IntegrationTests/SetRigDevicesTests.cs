using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MNX.MonitoringCenter.Inventory.Contracts.Devices;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Cpu;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Information;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Restrictions;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;
using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.UseCases.Mining.MiningDevice;
using MNX.MonitoringCenter.Management.UseCases.Overclocking.Presets;
using MNX.MonitoringCenter.Management.UseCases.SetRigDevices;

namespace MNX.MonitoringCenter.Management.UseCases.IntegrationTests;

public class SetRigDevicesTests : BaseTest
{
    private IMediator _mediator;

    private readonly static Guid OWNER_ID = Guid.Parse("59929758-14d1-43b5-a80c-7e84bfc45145");

    private readonly static Guid RIG_ID_1 = Guid.Parse("d444526c-ec2f-4b5a-af0e-3c516cd7aa83");
    private readonly static Guid RIG_ID_2 = Guid.Parse("2a287754-942e-476a-b0ab-0811e9b27416");

    [SetUp]
    public void SetUp()
    {
        _mediator = ServiceProvider.GetRequiredService<IMediator>();
    }

    [TestCaseSource(typeof(SetRigDevicesTestCase), nameof(SetRigDevicesTestCase.SetRigDevicesMessages))]
    public async Task SetRigDevices(TestCaseData data)
    {
        var rigRepository = ServiceProvider.GetRequiredService<IRigRepository>();
        var miningDeviceRepository = ServiceProvider.GetRequiredService<IMiningDeviceRepository>();
        var presetsRepository = ServiceProvider.GetRequiredService<IPresetRepository>();

        var command = new SetRigDevicesCommand(data.Command.RigId, data.Command.RigOwnerId, data.Command.Gpus, data.Command.Cpus);
        var result = await _mediator.Send(command);

        Assert.That(result.IsSuccess, Is.True, "Результат создания майнинг-устройств и их пресетов с разгонами в БД успешен");

        var deviceGpuId = Guid.Parse("10f81050-235f-464f-8724-c9cfcdd54551");
        var deviceGpuManufacterer = "AMD";
        var deviceGpuModel = "RX 580";

        var deviceCpuId = Guid.Parse("10f81050-235f-464f-8724-c9cfcdd54552");
        var deviceCpuManufacterer = "AMD";
        var deviceCpuModel = "Ryzen 5 7500f";

        switch (data.CaseNumber)
        {
            case 1:
                // Проверка значений видеокарты.
                var device_Gpu = await GetGpu(miningDeviceRepository.GetAvailable(new Specification(OWNER_ID)));
                CheckDevice(device_Gpu, RIG_ID_1, MiningDeviceType.GPU, deviceGpuManufacterer, deviceGpuModel);

                // Проверка пресета видеокарты.
                var preset_Gpu = await presetsRepository.GetById(device_Gpu.PresetId!.Value, OWNER_ID, default);
                CheckPreset(preset_Gpu, device_Gpu.Id, deviceGpuManufacterer, deviceGpuModel);

                // Проверка значений процессора.
                var device_Cpu = await GetCpu(miningDeviceRepository.GetAvailable(new Specification(OWNER_ID)));
                CheckDevice(device_Cpu, RIG_ID_1, MiningDeviceType.CPU, deviceCpuManufacterer, deviceCpuModel);

                // Проверка пресета процессора.
                var preset_Cpu = await presetsRepository.GetById(device_Cpu.PresetId!.Value, OWNER_ID, default);
                CheckPreset(preset_Cpu, device_Cpu.Id, deviceCpuManufacterer, deviceCpuModel);

                break;

            case 2:
                // Проверка значений видеокарты.
                device_Gpu = await GetGpu(miningDeviceRepository.GetAvailable(new Specification(OWNER_ID)));
                CheckDevice(device_Gpu, RIG_ID_2, MiningDeviceType.GPU, deviceGpuManufacterer, deviceGpuModel);

                // Проверка пресета видеокарты.
                preset_Gpu = await presetsRepository.GetById(device_Gpu.PresetId!.Value, OWNER_ID, default);
                CheckPreset(preset_Gpu, device_Gpu.Id, deviceGpuManufacterer, deviceGpuModel);

                // Проверка значений процессора.
                device_Cpu = await GetCpu(miningDeviceRepository.GetAvailable(new Specification(OWNER_ID)));
                CheckDevice(device_Cpu, RIG_ID_2, MiningDeviceType.CPU, deviceCpuManufacterer, deviceCpuModel);

                // Проверка пресета процессора.
                preset_Cpu = await presetsRepository.GetById(device_Cpu.PresetId!.Value, OWNER_ID, default);
                CheckPreset(preset_Cpu, device_Cpu.Id, deviceCpuManufacterer, deviceCpuModel);

                break;

            default:
                break;
        }

        // Проверить параметры девайса.
        void CheckDevice(MiningDeviceInfo device, Guid rigId, MiningDeviceType deviceType, string manufacterer, string model)
        {
            Assert.Multiple(() =>
            {
                Assert.That(device, Is.Not.Null,
                    "Майнинг-устройство было сохранено неверно.");

                Assert.That(device.RigId, Is.Not.Null.And.EqualTo(rigId),
                    $"Риг, на котором стоит устройство, не соответствует ожидаемому <<{rigId}>>.");

                Assert.That(device.OwnerId, Is.Not.Null.And.EqualTo(OWNER_ID),
                    "Идентификатор владельца некорректен.");

                Assert.That(device!.PresetId, Is.Not.Null,
                    "У устройства должен быть пресет.");

                Assert.That(device.FlightSheetId, Is.Null,
                    "Полетный лист устройства должен сбрасываться.");

                Assert.That(device.FlightSheetConfirmationState, Is.EqualTo(FlightSheetConfirmationState.Successfully),
                    "Состояние полетного листа устройство должно быть подтвержденным.");

                Assert.That(device.LifeCycleStatus, Is.EqualTo(MiningDeviceLifeCycleStatus.Online),
                    "Состояние устройства должно быть <<в сети>>");

                Assert.That(device.Name, Is.EqualTo($"{manufacterer} {model}"),
                    $"Модель и производитель устройства не совпадают с ожидаемыми <<{manufacterer} {model}>>.");

                Assert.That(device.Type, Is.EqualTo(deviceType),
                    $"Тип устройства не совпадает с ожидаемым <<{deviceType}>>");
            });
        }

        // Проверить параметры пресета.
        void CheckPreset(Preset preset, Guid deviceId, string manufacterer, string model)
        {
            Assert.Multiple(() =>
            {
                Assert.That(preset, Is.Not.Null,
                    "Пресет не был добавлен.");

                Assert.That(preset.UserId, Is.EqualTo(OWNER_ID),
                    "Идентификатор пользователя некорректен.");

                Assert.That(preset!.Name, Is.EqualTo(deviceId.ToString()),
                    $"Наименование пресета не совпадает с идентификатором устройства <<{deviceId}>>.");

                Assert.That(preset!.DeviceName, Is.EqualTo($"{manufacterer} {model}"),
                    $"Модель и производитель устройства не совпадают с ожидаемыми <<{manufacterer} {model}>>.");

                Assert.That(preset.IsVisible, Is.False,
                    "Данный пресет должен быть невидимым.");
            });
        }

        // Получить видеокарту из асинхронной коллекции.
        async Task<MiningDeviceInfo?> GetGpu(IAsyncEnumerable<MiningDeviceInfo> collection)
        {
            await foreach (var item in collection)
            {
                if (item.Type == MiningDeviceType.GPU)
                    return item;
            }
            return null;
        }

        // Получить процессор из асинхронной коллекции. 
        async Task<MiningDeviceInfo?> GetCpu(IAsyncEnumerable<MiningDeviceInfo> collection)
        {
            var list = new List<MiningDeviceInfo>();
            await foreach (var item in collection)
            {
                if (item.Type == MiningDeviceType.CPU)
                    return item;
            }
            return null;
        }
    }

    /// <summary>
    /// Источник данных интеграционного теста.
    /// </summary>
    private class SetRigDevicesTestCase
    {
        public static IEnumerable<TestCaseData> SetRigDevicesMessages
        {
            get
            {
                yield return new TestCaseData()
                {
                    Command = new SetRigDevicesCommand(
                        RigId: RIG_ID_1,
                        RigOwnerId: OWNER_ID,
                        Gpus: new List<Gpu>()
                        {
                            new Gpu()
                            {
                                Id = Guid.Parse("10f81050-235f-464f-8724-c9cfcdd54551"),
                                Pci = new() { Id = 1, Bus = "00:01.0"},
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
                                Overclocking = new Inventory.Contracts.Devices.Gpu.GpuOverclocking()
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
                        Cpus: new List<Cpu>()
                        {
                            new Cpu()
                            {
                                Id = Guid.Parse("10f81050-235f-464f-8724-c9cfcdd54552"),
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
                                Overclocking = new Inventory.Contracts.Devices.Cpu.CpuOverclocking()
                                {
                                    CoreClockLock = 2500,
                                    CoreVoltage = 25
                                }
                            }
                        }
                    ),
                    CaseNumber = 1
                };

                yield return new TestCaseData()
                {
                    Command = new SetRigDevicesCommand(
                        RigId: RIG_ID_2,
                        RigOwnerId: OWNER_ID,
                        Gpus: new List<Gpu>()
                        {
                            new Gpu()
                            {
                                Id = Guid.Parse("10f81050-235f-464f-8724-c9cfcdd54551"),
                                Pci = new() { Id = 1, Bus = "00:01.0"},
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
                                Overclocking = new Inventory.Contracts.Devices.Gpu.GpuOverclocking()
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
                        Cpus: new List<Cpu>()
                        {
                            new Cpu()
                            {
                                Id = Guid.Parse("10f81050-235f-464f-8724-c9cfcdd54552"),
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
                                Overclocking = new Inventory.Contracts.Devices.Cpu.CpuOverclocking()
                                {
                                    CoreClockLock = 2500,
                                    CoreVoltage = 25
                                }
                            }
                        }
                    ),
                    CaseNumber = 2
                };
            }
        }
    }
}

/// <summary>
/// Кейс данных для тестирования.
/// </summary>
public class TestCaseData
{
    /// <summary>
    /// Команда на назначение майнинг-девайсов при включении рига.
    /// </summary>
    public required SetRigDevicesCommand Command { get; init; }

    /// <summary>
    /// Номер кейса.
    /// </summary>
    public required int CaseNumber { get; init; }
}
