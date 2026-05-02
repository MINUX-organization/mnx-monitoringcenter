using MNX.MonitoringCenter.Management.Tests.Service.Assertions;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders;

namespace MNX.MonitoringCenter.Management.DataAccess.Tests.Miner;

using Miner = Core.Mining.Miner.Miner;

public partial class MinerRepositoryTests
{
    [TestCaseSource(typeof(MinersTestCaseSource), nameof(MinersTestCaseSource.CustomMiners))]
    public async Task Add_ValidCustomMiner_ShouldAddEntity(Miner data)
    {
        // Arrange

        var minerId = data.Id;


        // Act

        await _minerRepository.Add(data, default);
        var checkingMiner = await _minerRepository
            .GetMinerById(minerId, default);

        // Assert

        checkingMiner.ShouldBeEqualTo(data);
    }

    [TestCaseSource(typeof(MinersTestCaseSource), nameof(MinersTestCaseSource.CustomMiners))]
    public async Task Edit_ValidMiner_ShouldEditEntity(Miner data)
    {
        // Arrange

        var minerId = data.Id;
        var newMiner = CreateNewMiner(data);

        await PrepareDataBase(data);
        ClearChangeTracker();


        // Act

        await _minerRepository.Edit(newMiner, default);
        var checkingMiner = await _minerRepository
            .GetMinerById(minerId, default);


        // Assert

        checkingMiner.ShouldBeEqualTo(newMiner);
    }

    [TestCaseSource(typeof(MinersTestCaseSource), nameof(MinersTestCaseSource.CustomMiners))]
    public async Task Remove_ValidIdAndUserId_ShouldRemoveEntity(Miner data)
    {
        // Arrange

        var minerId = data.Id;
        var userId = MinersTestCaseSource.UserId;
        await PrepareDataBase(data);


        // Act

        await _minerRepository.Remove(minerId, userId, default);
        var checkingMiner = await _minerRepository
            .GetMinerById(minerId, default);


        // Assert

        Assert.That(checkingMiner, Is.Null);
    }

    private static Miner CreateNewMiner(Miner miner)
    {
        var minerBuilder = new MinerBuilder()
            .WithId(miner.Id)
            .WithOwner(MinersTestCaseSource.UserId)
            .WithName("NewMinerName")
            .WithVersion("2.0.1");

        if (miner.SupportedAlgorithms.Count > 0)
        {
            foreach (var algorithm in miner.SupportedAlgorithms)
            {
                minerBuilder.AddAlgorithm(algo =>
                    algo.WithAlgorithmId(algorithm.AlgorithmId));
            }
        }

        return minerBuilder.Build();
    }
}
