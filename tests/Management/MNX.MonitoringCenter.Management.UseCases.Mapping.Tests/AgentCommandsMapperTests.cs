using MNX.MonitoringCenter.Management.Core.Mining.Miner.Enums;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;
using MNX.MonitoringCenter.Management.Tests.Service.Assertions;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.ContractBuilders;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders.MiningConfigs;
using MNX.MonitoringCenter.Management.UseCases.Mapping.AgentCommands;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Pool;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Wallet;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Events;
using MNX.MonitoringCenter.Management.UseCases.Mining.Pool;
using MNX.MonitoringCenter.Management.UseCases.Mining.Wallet;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Tests;

[TestFixture]
public sealed class AgentCommandsMapperTests
{
    private IAgentCommandsMapper _agentCommandsMapper;
    private IPoolMapper _poolMapper;
    private IWalletMapper _walletMapper;

    [SetUp]
    public void SetUp()
    {
        _poolMapper = new PoolMapper();
        _walletMapper = new WalletMapper();
        _agentCommandsMapper = new AgentCommandsMapper(_poolMapper, _walletMapper);
    }

    [TestCaseSource(typeof(DeviceFLightSheetCollectionsTestData), nameof(DeviceFLightSheetCollectionsTestData.DevicesTestData))]
    public void MapToWorkerSettings_ValidFlightSheetList_ReturnsWorkerSettings(List<DeviceFLightSheet> data)
    {
        // Arrange
        // Act

        var mappedData = _agentCommandsMapper.MapToWorkerSettings(data);


        // Assert

        mappedData.ShouldBeEqual(data);
    }

    private class DeviceFLightSheetCollectionsTestData
    {
        public static IEnumerable<List<DeviceFLightSheet>> DevicesTestData
        {
            get
            {
                var flightSheet1Id = Guid.NewGuid();
                var flightSheet2Id = Guid.NewGuid();
                var flightSheet3Id = Guid.NewGuid();
                var guids = new DomainGuids()
                {
                    CryptocurrencyId = Guid.NewGuid(),
                    RigId = Guid.NewGuid(),
                    MinerId = Guid.NewGuid(),
                    PoolId = Guid.NewGuid(),
                    WalletId = Guid.NewGuid(),
                    UserId = Guid.NewGuid(),
                    AlgorithmId = Guid.NewGuid(),
                };

                yield return new List<DeviceFLightSheet>()
                {
                    new DeviceFLightSheet(
                        new MiningDeviceModelBuilder()
                            .WithManufacturer("AMD")
                            .WithModel("RX 6700 XT")
                            .WithRigId(guids.RigId)
                            .WithType("GPU")
                            .WithFlightSheetId(flightSheet1Id)
                            .WithFlightSheetName("FlightSheet1")
                            .WithFlightSheetConfirmationState(FlightSheetConfirmationState.Unconfirmed)
                            .WithPresetName("Preset1")
                            .WithMinerName("Miner")
                            .WithMinerVersion("1.0.1")
                            .Build(),
                        new FlightSheetBuilder()
                            .WithId(flightSheet1Id)
                            .WithName("FlightSheet1")
                            .AddTarget(target =>
                                target.WithMiningConfig(() =>
                                      {
                                          return new GpuMiningConfigBuilder()
                                            .WithAdditionalArguments("Argument1, Argument2")
                                            .WithConfigFileContent("Config:\n{\n\tSubConfig1: 1,\n\tSubConfig2: 2\n}")
                                            .AddCoinConfig(coinConfig => CreateMiningConfig(guids, 1, 1))
                                            .Build();
                                      })
                                      .WithMiner(miner =>
                                        miner.WithId(guids.MinerId)
                                             .WithName("Miner")
                                             .WithVersion("1.0.1")
                                             .WithOwner(guids.UserId)
                                             .WithSupportedDevices(DeviceTypeManufacturerCombination.AmdGpu)
                                             .WithMiningMode(MiningModeEnum.Single)))
                            .Build()
                    ),
                    new DeviceFLightSheet(
                        new MiningDeviceModelBuilder()
                            .WithManufacturer("Nvidia")
                            .WithModel("RTX 4060 ti")
                            .WithRigId(guids.RigId)
                            .WithType("GPU")
                            .WithFlightSheetId(flightSheet2Id)
                            .WithFlightSheetName("FlightSheet2")
                            .WithFlightSheetConfirmationState(FlightSheetConfirmationState.Successfully)
                            .WithPresetName("Preset2")
                            .WithMinerName("Miner")
                            .WithMinerVersion("1.0.2")
                            .Build(),
                        new FlightSheetBuilder()
                            .WithId(flightSheet2Id)
                            .AddTarget(target =>
                                target.WithMiningConfig(() =>
                                      {
                                          return new GpuMiningConfigBuilder()
                                            .WithAdditionalArguments("Argument1, Argument2")
                                            .WithConfigFileContent("Config:\n{\n\tSubConfig1: 1,\n\tSubConfig2: 2\n}")
                                            .AddCoinConfig(coinConfig => CreateMiningConfig(guids, 1, 2))
                                            .AddCoinConfig(coinConfig => CreateMiningConfig(guids, 2, 2))
                                            .Build();
                                      })
                                      .WithMiner(miner =>
                                        miner.WithId(guids.MinerId)
                                             .WithName("Miner")
                                             .WithVersion("1.0.2")
                                             .WithOwner(guids.UserId)
                                             .WithSupportedDevices(DeviceTypeManufacturerCombination.NvidiaGpu)
                                             .WithMiningMode(MiningModeEnum.Dual)))
                            .Build()),
                    new DeviceFLightSheet(
                        new MiningDeviceModelBuilder()
                            .WithManufacturer("Intel")
                            .WithModel("Core i9-149000K")
                            .WithRigId(guids.RigId)
                            .WithType("CPU")
                            .WithFlightSheetId(flightSheet3Id)
                            .WithFlightSheetName("FlightSheet3")
                            .WithFlightSheetConfirmationState(FlightSheetConfirmationState.Error)
                            .WithPresetName("Preset")
                            .WithMinerName("Miner")
                            .WithMinerVersion("1.0.3")
                            .Build(),
                        new FlightSheetBuilder()
                            .WithId(flightSheet3Id)
                            .AddTarget(target =>
                                target.WithMiner(miner =>
                                        miner.WithId(guids.MinerId)
                                             .WithOwner(guids.UserId)
                                             .WithSupportedDevices(DeviceTypeManufacturerCombination.IntelCpu)
                                             .WithMiningMode(MiningModeEnum.Triple))
                                      .WithMiningConfig(() =>
                                      {
                                          return new CpuMiningConfigBuilder()
                                            .WithAdditionalArguments("Argument1, Argument2")
                                            .WithConfigFileContent("Config:\n{\n\tSubConfig1: 1,\n\tSubConfig2: 2\n}")
                                            .AddCoinConfig(coinConfig => CreateMiningConfig(guids, 1, 3))
                                            .AddCoinConfig(coinConfig => CreateMiningConfig(guids, 2, 3))
                                            .AddCoinConfig(coinConfig => CreateMiningConfig(guids, 3, 3))
                                            .Build();
                                      }))
                            .Build()
                    )
                };
            }
        }

        private static MiningCoinConfigBuilder CreateMiningConfig(DomainGuids guids, int index, int number)
        {
            var a = new MiningCoinConfigBuilder()
                .WithPool(pool =>
                    pool.WithId(guids.PoolId)
                        .WithTls()
                        .WithOwner(guids.UserId)
                        .WithCryptocurrency(crypto =>
                            crypto.WithId(guids.CryptocurrencyId)
                                  .WithOwner(guids.UserId)
                                  .WithAlgorithm(algo =>
                                    algo.WithId(guids.AlgorithmId)
                                        .WithOwner(guids.UserId))))
                .WithWallet(wallet =>
                    wallet.WithId(guids.WalletId)
                          .WithCryptocurrency(crypto =>
                              crypto.WithId(guids.CryptocurrencyId)
                                    .WithOwner(guids.UserId)
                                    .WithAlgorithm(algo =>
                                      algo.WithId(guids.AlgorithmId)
                                          .WithOwner(guids.UserId))))
                .WithPoolPassword("654321");
            return a;
        }

        private readonly struct DomainGuids
        {
            public Guid WalletId { get; init; }
            public Guid PoolId { get; init; }
            public Guid UserId { get; init; }
            public Guid CryptocurrencyId { get; init; }
            public Guid AlgorithmId { get; init; }
            public Guid RigId { get; init; }
            public Guid MinerId { get; init; }
        }
    }
}
