using MNX.MonitoringCenter.Management.Tests.Service.Assertions;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders;

namespace MNX.MonitoringCenter.Management.DataAccess.Tests.Algorithm;

public partial class AlgorithmRepositoryTests
{
    [Test]
    public async Task GetById_ValidIdAndUserId_ReturnsUsersAlgorithm()
    {
        // Arrange

        var algorithmId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var algorithm = new AlgorithmBuilder()
            .WithId(algorithmId)
            .WithOwner(userId)
            .Build();

        await _algorithmRepository.AddAsync(algorithm);


        // Act

        var checkingAlgorithm = await _algorithmRepository.GetById(
            algorithmId, userId, default);


        // Assert

        checkingAlgorithm.ShouldBeEqualTo(algorithm);
    }

    [Test]
    public async Task GetById_ValidId_ReturnsDomainAlgorithm()
    {
        // Arrange

        var algorithmId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var algorithm = new AlgorithmBuilder()
            .WithId(algorithmId)
            .Build();

        await _algorithmRepository.AddAsync(algorithm);

        // Act

        var checkingAlgorithm = await _algorithmRepository.GetById(algorithmId, userId, default);


        // Assert

        checkingAlgorithm.ShouldBeEqualTo(algorithm);
    }

    [Test]
    public async Task GetById_ValidIdAndUserId_ReturnsNull()
    {
        // Arrange

        var algorithmId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        // Act

        var checkingAlgorithm = await _algorithmRepository
            .GetById(algorithmId, userId, default);


        // Assert

        Assert.That(checkingAlgorithm, Is.Null);
    }

    [Test]
    public async Task GetById_ValidIdAndInvalidUserId_ReturnsNull()
    {
        // Arrange

        var algorithmId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var algorithm = new AlgorithmBuilder()
            .WithId(algorithmId)
            .WithOwner()
            .Build();

        await _algorithmRepository.AddAsync(algorithm);

        // Act

        var checkingAlgorithm = await _algorithmRepository
            .GetById(algorithmId, userId, default);


        // Assert

        Assert.That(checkingAlgorithm, Is.Null);
    }
}
