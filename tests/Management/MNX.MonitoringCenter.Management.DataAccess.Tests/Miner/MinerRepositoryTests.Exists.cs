namespace MNX.MonitoringCenter.Management.DataAccess.Tests.Miner;

using Miner = Core.Mining.Miner.Miner;

public partial class MinerRepositoryTests
{
    [TestCaseSource(typeof(MinersTestCaseSource), nameof(MinersTestCaseSource.CustomMiners))]
    public async Task Exists_ValidUserIdMinerNameAndMinerVersion_ReturnsTrue(Miner data)
    {
        // Arrange

        var userId = MinersTestCaseSource.UserId;
        var minerName = data.Name;
        var minerVersion = data.Version;
        await PrepareDataBase(data);


        // Act

        var isExists = await _minerRepository
            .Exists(userId, minerName, minerVersion, default);


        // Assert

        Assert.That(isExists, Is.True);
    }

    [TestCaseSource(typeof(MinersTestCaseSource), nameof(MinersTestCaseSource.CustomMiners))]
    public async Task Exists_InvalidUserIdMinerNameAndMinerVersion_ReturnsFalse(Miner data)
    {
        // Arrange

        var userId = Guid.NewGuid();
        var minerName = "SomeInvalidMinerName";
        var minerVersion = "SomeInvalidMinerVersion";
        await PrepareDataBase(data);


        // Act

        var isExists = await _minerRepository
            .Exists(userId, minerName, minerVersion, default);


        // Assert

        Assert.That(isExists, Is.False);
    }

    [TestCaseSource(typeof(MinersTestCaseSource), nameof(MinersTestCaseSource.Miners))]
    public async Task Exists_ValidId_ReturnsTrue(Miner data)
    {
        // Arrange

        var minerd = data.Id;
        await PrepareDataBase(data);


        // Act

        var isExists = await _minerRepository
            .Exists(minerd, default);


        // Assert

        Assert.That(isExists, Is.True);
    }

    [TestCaseSource(typeof(MinersTestCaseSource), nameof(MinersTestCaseSource.Miners))]
    public async Task Exists_InvalidId_ReturnsTrue(Miner data)
    {
        // Arrange

        var minerd = Guid.NewGuid();
        await PrepareDataBase(data);


        // Act

        var isExists = await _minerRepository
            .Exists(minerd, default);


        // Assert

        Assert.That(isExists, Is.False);
    }

    [TestCaseSource(typeof(MinersTestCaseSource), nameof(MinersTestCaseSource.CustomMiners))]
    public async Task Exists_NonmatchingMinerIdAndValidUserIdAndMinerName_ReturnsTrue(Miner data)
    {
        // Arrange

        var minerId = Guid.NewGuid();
        var userId = MinersTestCaseSource.UserId;
        var minerName = data.Name;
        await PrepareDataBase(data);


        // Act

        var isExists = await _minerRepository
            .Exists(minerId, userId, minerName, default);


        // Assert

        Assert.That(isExists, Is.True);
    }

    [TestCaseSource(typeof(MinersTestCaseSource), nameof(MinersTestCaseSource.CustomMiners))]
    public async Task Exists_MatchingMinerIdAndValidUserIdAndMinerName_ReturnsFalse(Miner data)
    {
        // Arrange

        var minerId = data.Id;
        var userId = MinersTestCaseSource.UserId;
        var minerName = data.Name;
        await PrepareDataBase(data);


        // Act

        var isExists = await _minerRepository
            .Exists(minerId, userId, minerName, default);


        // Assert

        Assert.That(isExists, Is.False);
    }
}
