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
                MemoryClock = 1313,
                CoreClock = 2235,
                PowerLimit = 450,
                CriticalTemperature = 105,
                FanSpeed = 99,
            },

            new Preset
            {
                Id = Guid.Parse("eba6b890-2b31-4e73-8908-00170558ea32"),
                GpuName = "GeForce GTX 1660 Super",
                MemoryClock = 1313,
                CoreClock = 1530,
                PowerLimit = 100,
                CriticalTemperature = 90,
                FanSpeed = 80,
            },

            new Preset
            {
                Id = Guid.Parse("33fef879-06ff-420d-989a-a3b362129d77"),
                GpuName = "GeForce GTX 1660 Super",
                MemoryClock = 1750,
                CoreClock = 1785,
                PowerLimit = 125,
                CriticalTemperature = 105,
                FanSpeed = 99,
            }
        };

        var presetRepository = new Mock<IPresetRepository>();
        presetRepository
            .Setup(x => x.GetAllAvailable("GeForce GTX 1660 Super", TestHelper.Cryptocurrency.UserId))
            .Returns(presets.ToAsyncEnumerable());

        var handler = new GetPresetsQueryHandler(
            presetRepository.Object);

        var query = new GetPresetsQuery("GeForce GTX 1660 Super", 
                                        TestHelper.Cryptocurrency.UserId);

        var result = await handler.Handle(query, default).ToListAsync();

        Assert.NotNull(result);
        Assert.That(result, Is.EqualTo(presets));
    }
}
