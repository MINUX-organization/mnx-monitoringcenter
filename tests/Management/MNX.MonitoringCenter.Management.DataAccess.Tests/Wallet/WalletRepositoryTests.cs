using MNX.MonitoringCenter.Management.DataAccess.Tests.Infrastructure;
using MNX.MonitoringCenter.Management.DataAccess.Wallet;
using MNX.MonitoringCenter.Management.UseCases.Mining.Wallet;

namespace MNX.MonitoringCenter.Management.DataAccess.Tests.Wallet;

public partial class WalletRepositoryTests : BaseTest
{
    private IWalletRepository _walletRepository;

    [SetUp]
    public void SetUp()
    {
        _walletRepository = new WalletRepository(Context);
    }
}
