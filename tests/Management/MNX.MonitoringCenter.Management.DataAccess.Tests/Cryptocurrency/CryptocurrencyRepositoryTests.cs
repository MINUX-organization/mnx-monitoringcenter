using MNX.MonitoringCenter.Management.DataAccess.Cryptocurrency;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders;
using MNX.MonitoringCenter.Management.UseCases.Mining.Cryptocurrency;

namespace MNX.MonitoringCenter.Management.DataAccess.Tests.Cryptocurrency;

using Cryptocurrency = Core.Mining.Cryptocurrency;

public partial class CryptocurrencyRepositoryTests : BaseTest
{
    private ICryptocurrencyRepository _cryptocurrencyRepository;

    [SetUp]
    public void SetUp()
    {
        _cryptocurrencyRepository = new CryptocurrencyRepository(Context);
    }

    private static class CryptocurrencyTestCaseSource
    {
        public static Guid UserId = Guid.NewGuid();

        public static IEnumerable<List<Cryptocurrency>> CryptocurrencyList
        {
            get
            {
                yield return
                [
                    new CryptocurrencyBuilder()
                        .WithOwner(UserId)
                        .WithAlgorithm(algo =>
                            algo.WithOwner(UserId))
                        .Build(),
                    new CryptocurrencyBuilder()
                        .WithOwner(UserId)
                        .WithAlgorithm(algo =>
                            algo.WithOwner(UserId))
                        .Build(),
                    new CryptocurrencyBuilder()
                        .WithAlgorithm(algo =>
                            algo.WithName("DomainAlgorithm_1"))
                        .Build()
                ];
            }
        }
    }
}
