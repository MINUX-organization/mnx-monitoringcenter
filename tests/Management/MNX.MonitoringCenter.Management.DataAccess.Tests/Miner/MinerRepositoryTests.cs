using MNX.MonitoringCenter.Management.DataAccess.Miner;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders;
using MNX.MonitoringCenter.Management.UseCases.Mining.Miner;

namespace MNX.MonitoringCenter.Management.DataAccess.Tests.Miner;

using Miner = Core.Mining.Miner.Miner;

public partial class MinerRepositoryTests : BaseTest
{
    private IMinerRepository _minerRepository;

    [SetUp]
    public void SetUp()
    {
        _minerRepository = new MinerRepository(Context);
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
