using MNX.MonitoringCenter.Management.Contracts.Miner;
using MNX.MonitoringCenter.Management.Core.Mining.Miner.Enums;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Miner;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Tests.Builders.ContractBuilders;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Tests.Builders.CoreBuilders;
using MNX.MonitoringCenter.Management.UseCases.Mining.Miner;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Tests;

using Algorithm = Core.Mining.Miner.MinerAlgorithm;

[TestFixture]
public sealed class MinerMapperTests
{
    private IMinerMapper _minerMapper;

    [SetUp]
    public void SetUp()
    {
        _minerMapper = new MinerMapper();
    }

    [Test]
    public void MapToCoreEntity_ValidMinerInputModel_ReturnMiner()
    {
        // Arrange
        var inputModel = new MinerInputModelBuilder()
            .WithSupportedDevices(DeviceTypeManufacturerCombination.NvidiaGpu)
            .WithMiningMode(MiningModeEnum.Dual)
            .Build();
        var userId = Guid.NewGuid();


        // Act

        var mappedEntity = _minerMapper.MapToCoreEntity(inputModel, userId);


        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(mappedEntity, Is.Not.Null);
            Assert.That(mappedEntity.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(mappedEntity.Name, Is.EqualTo(inputModel.Name));
            Assert.That(mappedEntity.Version, Is.EqualTo(inputModel.Version));
            Assert.That(mappedEntity.InstallationUrl, Is.EqualTo(inputModel.InstallationUrl));
            Assert.That(mappedEntity.SupportedDevices, Is.EqualTo(inputModel.SupportedDevices));
            Assert.That(mappedEntity.PoolTemplate, Is.EqualTo(inputModel.PoolTemplate));
            Assert.That(mappedEntity.WalletWorkerTemplate, Is.EqualTo(inputModel.WalletWorkerTemplate));
            Assert.That(mappedEntity.MiningMode, Is.EqualTo(inputModel.MiningMode));
            Assert.That(mappedEntity.OwnerId, Is.EqualTo(userId));
            Assert.That(mappedEntity.Type, Is.EqualTo(MinerTypeEnum.Custom));
            Assert.That(mappedEntity.SupportedAlgorithms, Has.Count.EqualTo(0));
        });
    }

    [Test]
    public void MapToModel_ValidMiner_ReturnMinerModel()
    {
        // Arrange

        var minerId = Guid.NewGuid();

        var miner = new MinerBuilder()
            .WithId(minerId)
            .WithOwner()
            .WithPoolTemplate("{template1, template2, template3, template4}")
            .WithWalletWorkerTemplate("{tempalte5, template6, template7}")
            .AddAlgorithm(algo => algo.WithMinerId(minerId))
            .AddAlgorithm(algo => algo.WithMinerId(minerId))
            .AddAlgorithm(algo => algo.WithMinerId(minerId))
            .Build();


        // Act

        var mappedModel = _minerMapper.MapToModel(miner);


        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(mappedModel, Is.Not.Null);
            Assert.That(mappedModel.Id, Is.EqualTo(miner.Id));
            Assert.That(mappedModel.Name, Is.EqualTo(miner.Name));
            Assert.That(mappedModel.Version, Is.EqualTo(miner.Version));
            Assert.That(mappedModel.InstallationUrl, Is.EqualTo(miner.InstallationUrl));
            Assert.That(mappedModel.MiningMode, Is.EqualTo(miner.MiningMode));
            Assert.That(mappedModel.SupportedDevices, Is.EqualTo(miner.SupportedDevices));
            Assert.That(mappedModel.OwnerId, Is.EqualTo(miner.OwnerId));
            Assert.That(mappedModel.PoolTemplate, Is.EqualTo(miner.PoolTemplate));
            Assert.That(mappedModel.WalletWorkerTemplate, Is.EqualTo(miner.WalletWorkerTemplate));
            Assert.That(mappedModel.SupportedAlgorithms, Is.Not.Null);
            Assert.That(mappedModel.SupportedAlgorithms, Is.Not.Empty);
            Assert.That(mappedModel.SupportedAlgorithms, Has.Count.EqualTo(miner.SupportedAlgorithms.Count));
            CheckSupportedAlgorithms(miner.SupportedAlgorithms, mappedModel.SupportedAlgorithms);
        });
    }

    private void CheckSupportedAlgorithms(List<Algorithm> expected, List<MinerAlgorithmModel> checking)
    {
        for (int i = 0; i < expected.Count; i++)
        {
            Assert.Multiple(() =>
            {
                Assert.That(checking[i].Id, Is.EqualTo(expected[i].AlgorithmId));
                Assert.That(checking[i].Name, Is.EqualTo(expected[i].Name));
            });
        }
    }
}
