using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders;

namespace MNX.MonitoringCenter.Management.DataAccess.Tests.MinerAlgorithm;

using MinerAlgorithm = Core.Mining.Miner.MinerAlgorithm;

public partial class MinerAlgorithmRepositoryTests
{
    [TestCaseSource(typeof(MinerAlgorithmsTestCaseSource), nameof(MinerAlgorithmsTestCaseSource.MinerAlgorithmsWithAlgorithmId))]
    public async Task GetMinerAlgorithmsByAlgorithmId_ValidAlgorithmId_ReturnsEntities(List<MinerAlgorithm> data)
    {
        // Arrange

        var algorithmId = MinerAlgorithmsTestCaseSource.AlgorithmId;
        var query = data.Where(x => x.AlgorithmId == algorithmId);
        var expectedCount = query.Count();

        await PrepareDataBase(algorithmId, data);
        await _minerAlgorithmRepository.AddRangeAsync(data);


        // Act

        var checkingMinerAlgorithms = await _minerAlgorithmRepository
            .GetMinerAlgorithmsByAlgorithmId(algorithmId).ToListAsync();


        // Assert

        Assert.That(checkingMinerAlgorithms, Is.Not.Null);
        Assert.That(checkingMinerAlgorithms, Has.Count.EqualTo(expectedCount));
        AssertMinerAlgorithms(data, checkingMinerAlgorithms);
    }

    [TestCaseSource(typeof(MinerAlgorithmsTestCaseSource), nameof(MinerAlgorithmsTestCaseSource.MinerAlgorithmsWithAlgorithmId))]
    public async Task GetMinerAlgorithmsByAlgorithmId_InvalidAlgorithmId_ReturnsEmpty(List<MinerAlgorithm> data)
    {
        // Arrange

        var algorithmId = Guid.NewGuid();
        var query = data.Where(x => x.AlgorithmId == algorithmId);
        var expectedCount = query.Count();

        await PrepareDataBase(algorithmId, data);
        await _minerAlgorithmRepository.AddRangeAsync(data);


        // Act

        var checkingMinerAlgorithms = await _minerAlgorithmRepository
            .GetMinerAlgorithmsByAlgorithmId(algorithmId).ToListAsync();


        // Assert

        Assert.That(checkingMinerAlgorithms, Is.Not.Null);
        Assert.That(checkingMinerAlgorithms, Has.Count.EqualTo(expectedCount));
        AssertMinerAlgorithms([.. query], checkingMinerAlgorithms);
    }

    private static void AssertMinerAlgorithms(List<MinerAlgorithm> expected, List<MinerAlgorithm> checking)
    {
        expected = [.. expected.OrderBy(x => x.Name)];
        checking = [.. checking.OrderBy(x => x.Name)];

        for (var i = 0; i < expected.Count; i++)
        {
            Assert.Multiple(() =>
            {
                Assert.That(checking[i].AlgorithmId, Is.EqualTo(expected[i].AlgorithmId));
                Assert.That(checking[i].MinerId, Is.EqualTo(expected[i].MinerId));
                Assert.That(checking[i].Name, Is.EqualTo(expected[i].Name));
            });
        }
    }
}
