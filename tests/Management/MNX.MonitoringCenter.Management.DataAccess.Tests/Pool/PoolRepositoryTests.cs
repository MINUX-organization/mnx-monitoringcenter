using MNX.MonitoringCenter.Management.DataAccess.Pool;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders;
using MNX.MonitoringCenter.Management.UseCases.Mining.Pool;

namespace MNX.MonitoringCenter.Management.DataAccess.Tests.Pool;

using Pool = Core.Mining.Pool;

public partial class PoolRepositoryTests : BaseTest
{
    private IPoolRepository _poolRepository;

    [SetUp]
    public void SetUp()
    {
        _poolRepository = new PoolRepository(Context);
    }

    private async Task PrepareDataBase(List<Pool> data)
    {
        foreach (var item in data)
            await _poolRepository.Add(item);
    }

    private static class PoolsTestCaseSource
    {
        public static Guid UserId = Guid.NewGuid();

        public static IEnumerable<List<Pool>> PoolLists
        {
            get
            {
                yield return
                [
                    new PoolBuilder()
                        .WithOwner(UserId)
                        .WithCryptocurrency(crypto =>
                            crypto.WithAlgorithm())
                        .Build(),
                    new PoolBuilder()
                        .WithOwner(UserId)
                        .WithCryptocurrency(crypto =>
                            crypto.WithAlgorithm())
                        .Build(),
                    new PoolBuilder()
                        .WithCryptocurrency(crypto =>
                            crypto.WithAlgorithm())
                        .Build(),
                ];
            }
        }

        public static IEnumerable<Pool> Pools
        {
            get
            {
                yield return new PoolBuilder()
                    .WithOwner(UserId)
                    .WithCryptocurrency(crypto => crypto.WithAlgorithm())
                    .Build();
                yield return new PoolBuilder()
                    .WithOwner(UserId)
                    .WithCryptocurrency(crypto => crypto.WithAlgorithm(algo => algo.WithOwner(UserId)))
                    .Build();
            }
        }

        public static IEnumerable<Pool> DomainPools
        {
            get
            {
                yield return new PoolBuilder()
                    .WithCryptocurrency(crypto => crypto.WithAlgorithm())
                    .Build();
            }
        }
    }
}
