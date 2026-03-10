using MNX.MonitoringCenter.Management.DataAccess.Miner;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders;
using MNX.MonitoringCenter.Management.UseCases.Mining.Miner;

namespace MNX.MonitoringCenter.Management.DataAccess.Tests.Miner;

using Miner = Core.Mining.Miner.Miner;
using MinerAlgorithm = Core.Mining.Miner.MinerAlgorithm;

public partial class MinerRepositoryTests : BaseTest
{
    private IMinerRepository _minerRepository;

    [SetUp]
    public void SetUp()
    {
        _minerRepository = new MinerRepository(Context);
    }

    private static void AssertMiners(List<Miner> expected, List<Miner> checking)
    {
        expected = [.. expected.OrderBy(x => x.Name)];
        checking = [.. checking.OrderBy(x => x.Name)];

        for (var i = 0; i < expected.Count; i++)
        {
            AssertMiner(expected[i], checking[i]);
        }
    }

    private static void AssertMiner(Miner expected, Miner checking)
    {
        Assert.Multiple(() =>
        {
            Assert.Multiple(() =>
            {
                Assert.That(checking.Id, Is.EqualTo(expected.Id));
                Assert.That(checking.Type, Is.EqualTo(expected.Type));
                Assert.That(checking.OwnerId, Is.EqualTo(expected.OwnerId));
                Assert.That(checking.Name, Is.EqualTo(expected.Name));
                Assert.That(checking.Version, Is.EqualTo(expected.Version));
                Assert.That(checking.InstallationUrl, Is.EqualTo(expected.InstallationUrl));
                Assert.That(checking.MiningMode, Is.EqualTo(expected.MiningMode));
                Assert.That(checking.PoolTemplate, Is.EqualTo(expected.PoolTemplate));
                Assert.That(checking.WalletWorkerTemplate, Is.EqualTo(expected.WalletWorkerTemplate));
                Assert.That(checking.SupportedDevices, Is.EqualTo(expected.SupportedDevices));
                AssertAlgorithms(expected.SupportedAlgorithms, checking.SupportedAlgorithms);
            });
        });
    }

    private static void AssertAlgorithms(List<MinerAlgorithm> expected, List<MinerAlgorithm> checking)
    {
        expected = [.. expected.OrderBy(x => x.Name)];
        checking = [.. checking.OrderBy(x => x.Name)];

        for (var i = 0; i < expected.Count; i++)
        {
            Assert.Multiple(() =>
            {
                Assert.That(checking[i].AlgorithmId, Is.EqualTo(expected[i].AlgorithmId));
                Assert.That(checking[i].MinerId, Is.EqualTo(expected[i].MinerId));
                Assert.That(checking[i].Name, Is.EqualTo(expected[i].Name));
            });
        }
    }

    private async Task PrepareDataBase(List<Miner> data)
    {
        foreach (var item in data)
        {
            await _minerRepository.Add(item, default);
        }
    }

    private async Task PrepareDataBase(Miner data)
    {
        await _minerRepository.Add(data, default);
    }

    private static class MinersTestCaseSource
    {
        public static Guid UserId = Guid.NewGuid();

        public static IEnumerable<List<Miner>> MinerLists
        {
            get
            {
                yield return
                [
                    new MinerBuilder()
                        .WithOwner(UserId)
                        .AddAlgorithm()
                        .AddAlgorithm()
                        .AddAlgorithm()
                        .Build(),
                    new MinerBuilder()
                        .WithOwner(UserId)
                        .AddAlgorithm()
                        .AddAlgorithm()
                        .Build(),
                    new MinerBuilder()
                        .WithOwner(UserId)
                        .AddAlgorithm()
                        .Build(),
                    new MinerBuilder()
                        .Build(),
                ];
            }
        }

        public static IEnumerable<Miner> Miners
        {
            get
            {
                yield return new MinerBuilder()
                    .WithOwner(UserId)
                    .AddAlgorithm()
                    .AddAlgorithm()
                    .AddAlgorithm()
                    .Build();
                yield return new MinerBuilder()
                    .WithOwner(UserId)
                    .AddAlgorithm()
                    .AddAlgorithm()
                    .Build();
                yield return new MinerBuilder()
                    .WithOwner(UserId)
                    .AddAlgorithm()
                    .Build();
                yield return new MinerBuilder()
                    .Build();
            }
        }

        public static IEnumerable<Miner> CustomMiners
        {
            get
            {
                yield return new MinerBuilder()
                    .WithOwner(UserId)
                    .AddAlgorithm()
                    .AddAlgorithm()
                    .AddAlgorithm()
                    .Build();
                yield return new MinerBuilder()
                    .WithOwner(UserId)
                    .AddAlgorithm()
                    .AddAlgorithm()
                    .Build();
                yield return new MinerBuilder()
                    .WithOwner(UserId)
                    .AddAlgorithm()
                    .Build();
                yield return new MinerBuilder()
                    .WithOwner(UserId)
                    .Build();
            }
        }
    }
}
