using MNX.MonitoringCenter.Management.Tests.Service.Assertions;
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

        checkingAlgorithm.ShouldBeEqualTo(algorithm);
    }

    [Test]
    public async Task Remove_ValidAlgorithm_ShouldRemoveEntity()
    {
        // Arrange

        var algorithmId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        await _algorithmRepository.AddAsync(new AlgorithmBuilder()
            .WithId(algorithmId)
            .WithOwner(userId)
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
        var newName = "NewAlgorithm_1";

        var newAlgorithm = new AlgorithmBuilder()
            .WithId(algorithmId)
            .WithOwner(userId)
            .WithName("NewAlgorithm_1")
            .Build();

        await _algorithmRepository.AddAsync(new AlgorithmBuilder()
            .WithId(algorithmId)
            .WithOwner(userId)
            .Build());


        // Act

        await _algorithmRepository.EditAlgorithmName(algorithmId, userId, newName);
        var checkingAlgorithm = await _algorithmRepository.GetById(algorithmId, userId, default);


        // Assert

        checkingAlgorithm.ShouldBeEqualTo(newAlgorithm);
    }
}
