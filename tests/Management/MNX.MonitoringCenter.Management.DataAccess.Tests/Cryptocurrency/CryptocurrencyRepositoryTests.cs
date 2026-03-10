using MNX.MonitoringCenter.Management.DataAccess.Cryptocurrency;
using MNX.MonitoringCenter.Management.UseCases.Mining.Cryptocurrency;

namespace MNX.MonitoringCenter.Management.DataAccess.Tests.Cryptocurrency;

public partial class CryptocurrencyRepositoryTests : BaseTest
{
    private ICryptocurrencyRepository _cryptocurrencyRepository;

    [SetUp]
    public void SetUp()
    {
        _cryptocurrencyRepository = new CryptocurrencyRepository(Context);
    }
}
