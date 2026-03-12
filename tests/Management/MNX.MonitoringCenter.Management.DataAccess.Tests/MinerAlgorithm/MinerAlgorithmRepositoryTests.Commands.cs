using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Management.Contracts.AlgorithmBinding;

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

        Assert.That(checkingMinerAlgorithms, Is.Not.Null);
        Assert.That(checkingMinerAlgorithms, Has.Count.EqualTo(data.Count));
        AssertMinerAlgorithms(data, checkingMinerAlgorithms);
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

        Assert.That(checkingMinerAlgorithms, Is.Not.Null);
        Assert.That(checkingMinerAlgorithms, Is.Not.Empty);
        AssertMinerAlgorithms(relativeBindings, checkingMinerAlgorithms, algorithmId);
    }

    private static void AssertMinerAlgorithms(List<RelativeNameBindingModel> expected, List<MinerAlgorithm> checking, Guid expectedAlgorithmId)
    {
        expected = [.. expected.OrderBy(x => x.RelativeName)];
        checking = [.. checking.OrderBy(x => x.Name)];

        for (var i = 0; i < expected.Count; i++)
        {
            Assert.Multiple(() =>
            {
                Assert.That(checking[i].AlgorithmId, Is.EqualTo(expectedAlgorithmId));
                Assert.That(checking[i].MinerId, Is.EqualTo(expected[i].MinerId));
                Assert.That(checking[i].Name, Is.EqualTo(expected[i].RelativeName));
            });
        }
    }

    private List<RelativeNameBindingModel> CreateRelativeNameBindingModels(Guid[] minerIds)
    {
        var result = new List<RelativeNameBindingModel>();
        for (var i = 0; i < minerIds.Length; i++)
            result.Add(new RelativeNameBindingModel($"NewAlgorithmName_{i}", minerIds[i]));
        return result;
    }
}
