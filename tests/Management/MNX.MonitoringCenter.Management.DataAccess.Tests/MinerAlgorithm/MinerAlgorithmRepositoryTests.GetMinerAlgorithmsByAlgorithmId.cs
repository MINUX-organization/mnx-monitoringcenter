using MNX.MonitoringCenter.Management.Tests.Service.Assertions;

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

        checkingMinerAlgorithms.ShouldBeEqualTo(data);
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

        checkingMinerAlgorithms.ShouldBeEqualTo(query);
    }
}
