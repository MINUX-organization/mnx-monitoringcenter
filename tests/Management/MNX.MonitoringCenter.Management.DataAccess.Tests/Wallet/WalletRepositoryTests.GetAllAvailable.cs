using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders;
using MNX.MonitoringCenter.Management.UseCases;

namespace MNX.MonitoringCenter.Management.DataAccess.Tests.Wallet;

public partial class WalletRepositoryTests
{
    [TestCaseSource(typeof(WalletsTestCaseSource), nameof(WalletsTestCaseSource.WalletLists))]
    public async Task GetAllAvailable_ValidUserId_ReturnsAvailableEntities(List<Wallet> data)
    {
        // Assert

        var userId = WalletsTestCaseSource.UserId;
        var specification = new Specification(userId);
        var expectedCount = data
            .Where(x => x.OwnerId == userId)
            .Count();

        foreach (var item in data)
        {
            await _walletRepository.Add(item);
        }


        // Act

        var checkingList = await _walletRepository
            .GetAllAvailable(specification)
            .ToListAsync();


        // Assert

        Assert.That(checkingList, Is.Not.Null);
        Assert.That(checkingList, Has.Count.EqualTo(expectedCount));

        AssertWallets(data.Where(x => x.OwnerId == userId).ToList(), checkingList);
    }

    private static void AssertWallets(List<Wallet> expected, List<Wallet> checking)
    {
        expected = expected.OrderBy(x => x.Name).ToList();
        checking = checking.OrderBy(x => x.Name).ToList();

        for (var i = 0; i < expected.Count; i++)
        {
            Assert.Multiple(() =>
            {
                Assert.That(checking[i].Id, Is.EqualTo(expected[i].Id));
                Assert.That(checking[i].OwnerId, Is.EqualTo(expected[i].OwnerId));
                Assert.That(checking[i].Name, Is.EqualTo(expected[i].Name));
                Assert.That(checking[i].Address, Is.EqualTo(expected[i].Address));
                Assert.That(checking[i].CryptocurrencyId, Is.EqualTo(expected[i].CryptocurrencyId));

                var expectedCryptocurrency = expected[i].Cryptocurrency;
                var checkingCryptocurrency = checking[i].Cryptocurrency;
                Assert.That(checkingCryptocurrency?.Id, Is.EqualTo(expectedCryptocurrency?.Id));
                Assert.That(checkingCryptocurrency?.OwnerId, Is.EqualTo(expectedCryptocurrency?.OwnerId));
                Assert.That(checkingCryptocurrency?.FullName, Is.EqualTo(expectedCryptocurrency?.FullName));
                Assert.That(checkingCryptocurrency?.ShortName, Is.EqualTo(expectedCryptocurrency?.ShortName));
                Assert.That(checkingCryptocurrency?.AlgorithmId, Is.EqualTo(expectedCryptocurrency?.AlgorithmId));
            });
        }
    }
}
