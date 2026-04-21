using MNX.MonitoringCenter.Management.Tests.Service.Assertions;
using MNX.MonitoringCenter.Management.UseCases;

namespace MNX.MonitoringCenter.Management.DataAccess.Tests.Wallet;

using Wallet = Core.Mining.Wallet;

public partial class WalletRepositoryTests
{
    [TestCaseSource(typeof(WalletsTestCaseSource), nameof(WalletsTestCaseSource.WalletLists))]
    public async Task GetAllAvailable_ValidUserId_ReturnsAvailableEntities(List<Wallet> data)
    {
        // Assert

        var userId = WalletsTestCaseSource.UserId;
        var specification = new Specification(userId);

        var query = data.Where(x => x.OwnerId == userId);

        await PrepareDataBase(data);

        // Зануляем алгоритмы в ожидаемом результате,
        // так как GetAllAvailable не возвращает Algorithm.
        foreach (var wallet in query)
        {
            wallet.Cryptocurrency!.Algorithm = null;
        }

        // Act

        var checkingList = await _walletRepository
            .GetAllAvailable(specification)
            .ToListAsync();


        // Assert

        checkingList.ShouldBeEqualTo(query);
    }
}
