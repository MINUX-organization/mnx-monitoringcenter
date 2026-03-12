using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders;

namespace MNX.MonitoringCenter.Management.DataAccess.Tests.Algorithm;

public partial class AlgorithmRepositoryTests
{
    [Test]
    public async Task AddAsync_ValidAlgorithm_ShoudAddEntity()
    {
        // Arrange

        var algorithmId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var algorithmName = "Algo_1";
        var algorithm = new AlgorithmBuilder()
            .WithId(algorithmId)
            .WithOwner(userId)
            .WithName(algorithmName)
            .Build();

        
        // Act

        await _algorithmRepository.AddAsync(algorithm);
        var checkingAlgorithm = await _algorithmRepository
            .GetById(algorithmId, userId, default);


        // Assert

        Assert.That(checkingAlgorithm, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(checkingAlgorithm.Id, Is.EqualTo(algorithmId));
            Assert.That(checkingAlgorithm.Name, Is.EqualTo(algorithmName));
            Assert.That(checkingAlgorithm.OwnerId, Is.EqualTo(userId));
        });
    }

    [Test]
    public async Task Remove_ValidAlgorithm_ShouldRemoveEntity()
    {
        // Arrange

        var algorithmId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var algorithmName = "Algo_1";

        await _algorithmRepository.AddAsync(new AlgorithmBuilder()
            .WithId(algorithmId)
            .WithOwner(userId)
            .WithName(algorithmName)
            .Build());


        // Act

        await _algorithmRepository.Remove(algorithmId, userId);
        var checkingAlgorithm = await _algorithmRepository
            .GetById(algorithmId, userId, default);
        

        // Assert
        Assert.That(checkingAlgorithm, Is.Null);
    }

    [Test]
    public async Task EditAlgorithmName_NewValidName_ShouldRenameEntity()
    {
        // Arrange

        var algorithmId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var oldName = "Algo_1";
        var newName = "NewAlgorithm_1";

        await _algorithmRepository.AddAsync(new AlgorithmBuilder()
            .WithId(algorithmId)
            .WithOwner(userId)
            .WithName(oldName)
            .Build());


        // Act

        await _algorithmRepository.EditAlgorithmName(algorithmId, userId, newName);
        var checkingAlgorithm = await _algorithmRepository.GetById(algorithmId, userId, default);


        // Assert

        Assert.That(checkingAlgorithm, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(checkingAlgorithm.Name, Is.EqualTo(newName));
            Assert.That(checkingAlgorithm.Id, Is.EqualTo(algorithmId));
            Assert.That(checkingAlgorithm.OwnerId, Is.EqualTo(userId));
        });
    }
}
