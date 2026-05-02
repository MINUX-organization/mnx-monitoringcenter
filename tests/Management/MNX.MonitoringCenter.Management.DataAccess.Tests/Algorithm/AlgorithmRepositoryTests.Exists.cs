using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders;

namespace MNX.MonitoringCenter.Management.DataAccess.Tests.Algorithm;

public partial class AlgorithmRepositoryTests
{
    [Test]
    public async Task Exists_UserIdAndName_ReturnsTrue()
    {
        // Arrange

        var userId = Guid.NewGuid();
        var name = "Algo_1";
        await _algorithmRepository.AddAsync(
            new AlgorithmBuilder()
                .WithName(name)
                .WithOwner(userId)
                .Build());


        // Act

        var isExists = await _algorithmRepository
            .Exists(userId, name, default);


        // Assert
        Assert.That(isExists, Is.True);
    }

    [Test]
    public async Task Exists_IdAndUserIdAndName_ReturnsTrue()
    {
        // Assert

        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var name = "Algo_1";
        await _algorithmRepository.AddAsync(
            new AlgorithmBuilder()
                .WithId(id)
                .WithName(name)
                .WithOwner(userId)
                .Build());


        // Act

        var isExists = await _algorithmRepository
            .Exists(id, name, userId, default);


        // Assert

        Assert.That(isExists, Is.False);
    }

    [Test]
    public async Task Exists_UserIdAndName_ReturnsFalse()
    {
        // Assert

        var userId = Guid.NewGuid();
        var name = "Algo_1";
        await _algorithmRepository.AddAsync(
            new AlgorithmBuilder().Build());


        // Act

        var isExists = await _algorithmRepository
            .Exists(userId, name, default);


        // Assert
        Assert.That(isExists, Is.False);
    }

    [Test]
    public async Task Exists_IdAndUserIdAndName_ReturnsFalse()
    {
        // Assert

        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var name = "Algo_1";
        await _algorithmRepository.AddAsync(
            new AlgorithmBuilder()
                .WithName(name)
                .WithOwner(userId)
                .Build());


        // Act

        var isExists = await _algorithmRepository
            .Exists(id, name, userId, default);


        // Assert
        Assert.That(isExists, Is.True);
    }
}
