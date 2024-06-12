using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;
using MNX.MonitoringCenter.Management.UseCases.Queries.GetPresetsQuery;
using MNX.MonitoringCenter.Management.UseCases.Tests.Commands;
using Moq;

namespace MNX.MonitoringCenter.Management.UseCases.Tests.Queries.Presets;

[TestFixture]
public class GetPresetsQueryHandlerTests
{
    [Test]
    public async Task GetPresetQuery_ReturnsQuery()
    {
        var presets = new List<Preset>
        {
            new Preset
            {
                Id = Guid.Parse("4d0b4812-6d2e-4d38-85c5-ac2c7871e000"),
                GpuName = "GeForce RTX 4090",
                Overclocking = new Overclocking
                {
                    CoreClockLock = 1000,
                    CoreClockOffset = 100,
                    CoreVoltage = 1000,
                    CoreVoltageOffset = 100,
                    MemoryClockLock = 1000,
                    MemoryClockOffset = 100,
                    MemoryVoltage = 1000,
                    MemoryVoltageOffset = 100,
                    CriticalTemperature = 120,
                    FanSpeed = 600,
                    PowerLimit = 800,
                    Id = new Guid()
                }
            },

            new Preset
            {
                Id = Guid.Parse("eba6b890-2b31-4e73-8908-00170558ea32"),
                GpuName = "GeForce GTX 1660 Super",
                Overclocking = new Overclocking
                {
                    CoreClockLock = 900,
                    CoreClockOffset = 100,
                    CoreVoltage = 900,
                    CoreVoltageOffset = 100,
                    MemoryClockLock = 1000,
                    MemoryClockOffset = 100,
                    MemoryVoltage = 900,
                    MemoryVoltageOffset = 100,
                    CriticalTemperature = 120,
                    FanSpeed = 600,
                    PowerLimit = 800,
                    Id = new Guid()
                }
            },

            new Preset
            {
                Id = Guid.Parse("33fef879-06ff-420d-989a-a3b362129d77"),
                GpuName = "GeForce GTX 1660 Super",
                Overclocking = new Overclocking
                {
                    CoreClockLock = 700,
                    CoreClockOffset = 100,
                    CoreVoltage = 900,
                    CoreVoltageOffset = 100,
                    MemoryClockLock = 1000,
                    MemoryClockOffset = 100,
                    MemoryVoltage = 400,
                    MemoryVoltageOffset = 100,
                    CriticalTemperature = 120,
                    FanSpeed = 600,
                    PowerLimit = 800,
                    Id = new Guid()
                }
            }
        };

        var presetRepository = new Mock<IPresetRepository>();
        presetRepository
            .Setup(x => x.GetAllAvailable("GeForce GTX 1660 Super", TestHelper.Cryptocurrency.UserId))
            .Returns(presets.ToAsyncEnumerable());

        var handler = new GetPresetsQueryHandler(
            presetRepository.Object, TestHelper.GetMapper());

        var query = new GetPresetsQuery("GeForce GTX 1660 Super", 
                                        TestHelper.Cryptocurrency.UserId);

        var result = await handler.Handle(query, default).ToListAsync();

        Assert.Multiple(() =>
        {
            Assert.That(result.ToAsyncEnumerable(), Is.InstanceOf<IAsyncEnumerable<PresetModel>>(),
                "Не совпадают типы");
            Assert.That(result, Is.Not.Empty, "Список пресетов пуст");

            Assert.That(result[0].Id, Is.EqualTo(presets[0].Id), "Коллекции не равны");
            Assert.That(result[1].Id, Is.EqualTo(presets[1].Id), "Коллекции не равны");
        });
    }
}
