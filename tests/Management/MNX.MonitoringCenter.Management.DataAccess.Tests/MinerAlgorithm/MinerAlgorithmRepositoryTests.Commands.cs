using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Management.Contracts.AlgorithmBinding;
using MNX.MonitoringCenter.Management.Tests.Service.Assertions;

namespace MNX.MonitoringCenter.Management.DataAccess.Tests.MinerAlgorithm;

using MinerAlgorithm = Core.Mining.Miner.MinerAlgorithm;

public partial class MinerAlgorithmRepositoryTests
{
    [TestCaseSource(typeof(MinerAlgorithmsTestCaseSource), nameof(MinerAlgorithmsTestCaseSource.MinerAlgorithmsWithAlgorithmId))]
    public async Task AddRangeAsync_ValidMinerAlgorithms_ShouldAddEntitiesList(List<MinerAlgorithm> data)
    {
        // Arrange

        var algorithmId = MinerAlgorithmsTestCaseSource.AlgorithmId;
        await PrepareDataBase(algorithmId, data);
        ClearChangeTracker();


        // Act

        await _minerAlgorithmRepository.AddRangeAsync(data);
        var checkingMinerAlgorithms = await _minerAlgorithmRepository
            .GetMinerAlgorithmsByAlgorithmId(algorithmId).ToListAsync();


        // Assert

        checkingMinerAlgorithms.ShouldBeEqualTo(data);
    }

    [TestCaseSource(typeof(MinerAlgorithmsTestCaseSource), nameof(MinerAlgorithmsTestCaseSource.MinerAlgorithmsWithAlgorithmId))]
    public async Task RemoveAllMinerAlgorithmsById_ValidAlgorithmId_ShouldRemoveEntitiesList(List<MinerAlgorithm> data)
    {
        // Arrange

        var algorithmId = MinerAlgorithmsTestCaseSource.AlgorithmId;
        await PrepareDataBase(algorithmId, data);
        await _minerAlgorithmRepository.AddRangeAsync(data);
        ClearChangeTracker();


        // Act

        await _minerAlgorithmRepository.RemoveAllMinerAlgorithmsById(algorithmId);
        var checkingMinerAlgorithms = await _minerAlgorithmRepository
            .GetMinerAlgorithmsByAlgorithmId(algorithmId).ToArrayAsync();


        // Assert

        Assert.That(checkingMinerAlgorithms, Is.Empty);
    }

    [TestCaseSource(typeof(MinerAlgorithmsTestCaseSource), nameof(MinerAlgorithmsTestCaseSource.MinerAlgorithmsWithAlgorithmId))]
    public async Task EditMinerBindingsByAlgorithmId_ValidAlgorithmIdAndBindings_ShouldEditEntitiesList(List<MinerAlgorithm> data)
    {
        // Arrange

        var algorithmId = MinerAlgorithmsTestCaseSource.AlgorithmId;
        await PrepareDataBase(algorithmId, data);
        await _minerAlgorithmRepository.AddRangeAsync(data);

        var relativeBindings = CreateRelativeNameBindingModels([.. data.Select(x => x.MinerId)]);
        ClearChangeTracker();


        // Act

        await _minerAlgorithmRepository.EditMinerBindingsByAlgorithmId(
            algorithmId,
            relativeBindings);
        var checkingMinerAlgorithms = await _minerAlgorithmRepository
            .GetMinerAlgorithmsByAlgorithmId(algorithmId).ToListAsync();


        // Assert

        relativeBindings = [.. relativeBindings.OrderBy(x => x.MinerId)];
        checkingMinerAlgorithms = [.. checkingMinerAlgorithms.OrderBy(x => x.MinerId)];

        Assert.That(checkingMinerAlgorithms, Is.Not.Null);
        Assert.That(checkingMinerAlgorithms, Is.Not.Empty);
        for (var i = 0; i < relativeBindings.Count; i++)
        {
            Assert.Multiple(() =>
            {
                Assert.That(checkingMinerAlgorithms[i].AlgorithmId, Is.EqualTo(algorithmId));
                Assert.That(checkingMinerAlgorithms[i].MinerId, Is.EqualTo(relativeBindings[i].MinerId));
                Assert.That(checkingMinerAlgorithms[i].Name, Is.EqualTo(relativeBindings[i].RelativeName));
            });
        }
    }

    private static List<RelativeNameBindingModel> CreateRelativeNameBindingModels(Guid[] minerIds)
    {
        var result = new List<RelativeNameBindingModel>();
        for (var i = 0; i < minerIds.Length; i++)
        {
            result.Add(new RelativeNameBindingModel($"NewAlgorithmName_{i}", minerIds[i]));
        }
        return result;
    }
}
