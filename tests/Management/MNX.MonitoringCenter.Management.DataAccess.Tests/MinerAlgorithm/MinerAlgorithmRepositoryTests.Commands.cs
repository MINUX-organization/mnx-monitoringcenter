using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Management.Contracts.AlgorithmBinding;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders;

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
        Context.ChangeTracker.Clear();


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
    public void AddRangeAsync_NonexistentMinerAlgorithms_ShouldThrowAnDbUpdateException(List<MinerAlgorithm> data)
    {
        // Arrange

        var algorithmId = MinerAlgorithmsTestCaseSource.AlgorithmId;

        
        // Act

        var exception = Assert.ThrowsAsync<DbUpdateException>(async () =>
            await _minerAlgorithmRepository.AddRangeAsync(data));


        // Assert

        Assert.That(exception, Is.Not.Null);
    }

    [TestCaseSource(typeof(MinerAlgorithmsTestCaseSource), nameof(MinerAlgorithmsTestCaseSource.MinerAlgorithmsWithAlgorithmId))]
    public async Task RemoveAllMinerAlgorithmsById_ValidAlgorithmId_ShouldRemoveEntitiesList(List<MinerAlgorithm> data)
    {
        // Arrange

        var algorithmId = MinerAlgorithmsTestCaseSource.AlgorithmId;
        await PrepareDataBase(algorithmId, data);
        await _minerAlgorithmRepository.AddRangeAsync(data);
        Context.ChangeTracker.Clear();


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
        Context.ChangeTracker.Clear();


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

    [TestCaseSource(typeof(MinerAlgorithmsTestCaseSource), nameof(MinerAlgorithmsTestCaseSource.MinerAlgorithmsWithAlgorithmId))]
    public async Task EditMinerBindingsByAlgorithmId_InvalidRelativeBindings_ShouldThrowAnDbUpdateException(List<MinerAlgorithm> data)
    {
        // Arrange

        var algorithmId = MinerAlgorithmsTestCaseSource.AlgorithmId;

        await PrepareDataBase(algorithmId, data);
        await _minerAlgorithmRepository.AddRangeAsync(data);

        var relativeBindings = CreateRelativeNameBindingModels(
        [
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
        ]);
        Context.ChangeTracker.Clear();


        // Act

        var exception = Assert.ThrowsAsync<DbUpdateException>(async () =>
            await _minerAlgorithmRepository.EditMinerBindingsByAlgorithmId(algorithmId, relativeBindings));


        // Assert

        Assert.That(exception, Is.Not.Null);
    }

    private async Task PrepareDataBase(Guid algorithmId, List<MinerAlgorithm> data)
    {
        await _algorithmRepository.AddAsync(new AlgorithmBuilder()
                .WithId(algorithmId)
                .Build());

        foreach (var item in data)
        {
            await _minerRepository.Add(new MinerBuilder()
                .WithId(item.MinerId)
                .Build(),
                default);
        }
    }

    private void AssertMinerAlgorithms(List<RelativeNameBindingModel> expected, List<MinerAlgorithm> checking, Guid expectedAlgorithmId)
    {
        expected = expected.OrderBy(x => x.RelativeName).ToList();
        checking = checking.OrderBy(x => x.Name).ToList();

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
