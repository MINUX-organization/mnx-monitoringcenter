using MNX.MonitoringCenter.Management.Core.Mining.Miner.Enums;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders.MiningConfigs;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders.MiningDevices;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders.Overclockings;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders.Overclockings.Fans;
using MNX.MonitoringCenter.Management.UseCases.Mapping.MiningDevice;
using MNX.MonitoringCenter.Management.UseCases.Mining.MiningDevice;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Tests;

[TestFixture]
public sealed class MiningDeviceMapperTests
{
    private IMiningDeviceMapper _miningDeviceMapper;

    [SetUp]
    public void SetUp()
    {
        _miningDeviceMapper = new MiningDeviceMapper();
    }

    [Test]
    public void MapToModel_ValidMiningDeviceInfo_ReturnMiningDeviceModel()
    {
        // Arrange

        var minerId = Guid.NewGuid();
        var flightSheetId = Guid.NewGuid();
        var checkingMinerName = "Miner1";
        var checkingMinerVersion = "1.0.1";
        var algorithm1Id = Guid.NewGuid();
        var algorithm2Id = Guid.NewGuid();
        var ownerId = Guid.NewGuid();

        var deviceInfo = new MiningDeviceInfoBuilder()
            .WithManufacturer("Nvidia")
            .WithModel("RTX 4060 ti")
            .WithRig()
            .WithOwner()
            .WithFlightSheet(flightSheet =>
                flightSheet.WithId(flightSheetId)
                           .WithOwnerId(ownerId)
                           .AddTarget(target =>
                                target.WithFlightSheetId(flightSheetId)
                                      .WithMiningConfig(() =>
                                      {
                                          return new GpuMiningConfigBuilder()
                                                .WithConfigFileContent("content")
                                                .WithAdditionalArguments("arguments")
                                                .AddCoinConfig(coinConfig =>
                                                    coinConfig.WithWallet(wallet =>
                                                                wallet.WithCryptocurrency(crypto =>
                                                                    crypto.WithAlgorithm(algo =>
                                                                        algo.WithId(Guid.NewGuid())))
                                                              .WithOwnerId(ownerId)))
                                                .Build();
                                      })
                                      .WithMiner(miner =>
                                            miner.WithId(minerId)
                                                 .WithName(checkingMinerName)
                                                 .WithVersion(checkingMinerVersion)
                                                 .WithMiningMode(MiningModeEnum.Triple)
                                                 .WithSupportedDevices(DeviceTypeManufacturerCombination.NvidiaGpu)
                                                 .AddAlgorithm(algo =>
                                                    algo.WithMinerId(minerId))))
                           .AddTarget(target =>
                                target.WithFlightSheetId(flightSheetId)
                                      .WithMiningConfig(() =>
                                      {
                                          return new CpuMiningConfigBuilder()
                                                .WithConfigFileContent("content")
                                                .WithAdditionalArguments("arguments")
                                                .WithHugePages(3)
                                                .WithThreads(6)
                                                .AddCoinConfig(coinConfig =>
                                                    coinConfig.WithWallet(wallet =>
                                                        wallet.WithCryptocurrency(crypto =>
                                                            crypto.WithAlgorithm(algo =>
                                                                algo.WithId(algorithm2Id)))
                                                              .WithOwnerId(ownerId)))
                                                .Build();
                                      })
                                      .WithMiner(miner =>
                                        miner.WithMiningMode(MiningModeEnum.Dual)
                                             .WithSupportedDevices(DeviceTypeManufacturerCombination.NvidiaGpu)
                                             .AddAlgorithm(algo =>
                                                algo.WithMinerId(minerId)))))
            .WithDeviceType(MiningDeviceType.GPU)
            .WithLifeCycleStatus(MiningDeviceLifeCycleStatus.Offline)
            .WithPreset(preset =>
                preset.WithDeviceName("Nvidia RTX 4060 ti")
                      .WithVisible()
                      .WithOwnerId(ownerId)
                      .WithOverclocking(() =>
                      {
                          return new NvidiaGpuOverclockingBuilder()
                            .WithFanOverclocking(() =>
                            {
                                return new FanOverclockingWithLinearDependenceBuilder()
                                    .AddTargetPoint(point =>
                                        point.WithPointIndex(0)
                                             .WithFanSpeedValueTarget(20)
                                             .WithTemperatureValueTarget(35))
                                    .AddTargetPoint(point =>
                                        point.WithPointIndex(1)
                                             .WithFanSpeedValueTarget(50)
                                             .WithTemperatureValueTarget(65))
                                    .AddTargetPoint(point =>
                                        point.WithPointIndex(2)
                                             .WithFanSpeedValueTarget(100)
                                             .WithTemperatureValueTarget(80))
                                    .Build();
                            })
                            .WithCoreClockOffset(250)
                            .WithMemoryClockOffset(2500)
                            .WithPowerLimit(108)
                            .Build();
                      }))
            .Build();
        


        // Act

        var mappedModel = _miningDeviceMapper.MapToModel(deviceInfo);


        // Assert

        Assert.Multiple(() =>
        {
            Assert.That(mappedModel, Is.Not.Null);
            Assert.That(mappedModel.Id, Is.EqualTo(deviceInfo.Id));
            Assert.That(mappedModel.Manufacturer, Is.EqualTo(deviceInfo.Manufacturer));
            Assert.That(mappedModel.Model, Is.EqualTo(deviceInfo.Model));
            Assert.That(mappedModel.Type, Is.EqualTo(deviceInfo.Type.ToString()));
            Assert.That(mappedModel.RigId, Is.EqualTo(deviceInfo.RigId));
            Assert.That(mappedModel.FlightSheetId, Is.EqualTo(deviceInfo.FlightSheetId));
            Assert.That(mappedModel.FlightSheetName, Is.EqualTo(deviceInfo.FlightSheet.Name));
            Assert.That(mappedModel.FlightSheetConfirmationState, Is.EqualTo(deviceInfo.FlightSheetConfirmationState));
            Assert.That(mappedModel.PresetName, Is.EqualTo(deviceInfo.Preset.Name));
            Assert.That(mappedModel.MinerName, Is.EqualTo(checkingMinerName));
            Assert.That(mappedModel.MinerVersion, Is.EqualTo(checkingMinerVersion));
            Assert.That(mappedModel.IsOnline, Is.EqualTo(deviceInfo.IsOnline));
        });
    }
}
