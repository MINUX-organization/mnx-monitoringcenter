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

    private static class PoolsTestCaseSource
    {
        public static Guid UserId = Guid.NewGuid();

        public static IEnumerable<List<Pool>> Pools
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
    }
}
