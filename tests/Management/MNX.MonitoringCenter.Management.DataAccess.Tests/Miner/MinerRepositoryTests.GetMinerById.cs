namespace MNX.MonitoringCenter.Management.DataAccess.Tests.Miner;

using MNX.MonitoringCenter.Management.Tests.Service.Assertions;
using Miner = Core.Mining.Miner.Miner;

public partial class MinerRepositoryTests
{
    [TestCaseSource(typeof(MinersTestCaseSource), nameof(MinersTestCaseSource.Miners))]
    public async Task GetMinerById_ValidId_ReturnsEntity(Miner data)
    {
        // Arrange

        var minerId = data.Id;
        await PrepareDataBase(data);


        // Act

        var checkingMiner = await _minerRepository
            .GetMinerById(minerId, default);


        // Assert

        checkingMiner.ShouldBeEqualTo(data);
    }

    [TestCaseSource(typeof(MinersTestCaseSource), nameof(MinersTestCaseSource.Miners))]
    public async Task GetMinerById_InvalidId_ReturnsNull(Miner data)
    {
        // Arrange

        var minerId = Guid.NewGuid();
        await PrepareDataBase(data);


        // Act

        var checkingMiner = await _minerRepository
            .GetMinerById(minerId, default);


        // Assert

        Assert.That(checkingMiner, Is.Null);
    }
}
