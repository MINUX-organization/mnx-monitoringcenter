using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders;
using Npgsql;
using System.Data.Common;

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
    public async Task AddAsync_ExistingAlgorithm_ShouldThrowAnDbUpdateException()
    {
        // Arrange

        var algorithmId_1 = Guid.NewGuid();
        var algorithmId_2 = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var algorithmName = "Algo_1";

        await _algorithmRepository.AddAsync(new AlgorithmBuilder()
            .WithId(algorithmId_1)
            .WithOwner(userId)
            .WithName(algorithmName)
            .Build());


        // Act

        var exception = Assert.ThrowsAsync<DbUpdateException>(async () =>
        {
            await _algorithmRepository.AddAsync(new AlgorithmBuilder()
                .WithId(algorithmId_2)
                .WithOwner(userId)
                .WithName(algorithmName)
                .Build());
        });

        // Assert

        Assert.That(exception, Is.Not.Null);
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

    [Test]
    public async Task EditAlgorithmName_NewExistingName_ShouldThrowAnPostgresException()
    {
        // Arrange

        var algorithmId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var algorithmName_1 = "Algo_1";
        var algorithmName_2 = "Algo_2";

        await _algorithmRepository.AddAsync(new AlgorithmBuilder()
            .WithOwner(userId)
            .WithName(algorithmName_2)
            .Build());
        await _algorithmRepository.AddAsync(new AlgorithmBuilder()
            .WithId(algorithmId)
            .WithOwner(userId)
            .WithName(algorithmName_1)
            .Build());


        // Act

        var exception = Assert.ThrowsAsync<PostgresException>(async () =>
        {
            await _algorithmRepository.EditAlgorithmName(algorithmId, userId, algorithmName_2);
        });


        // Assert

        Assert.That(exception, Is.Not.Null);
    }
}
