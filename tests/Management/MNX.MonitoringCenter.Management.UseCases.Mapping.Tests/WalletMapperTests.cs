using MNX.MonitoringCenter.Management.UseCases.Mapping.Wallet;
using MNX.MonitoringCenter.Management.UseCases.Mining.Wallet;
using MNX.MonitoringCenter.Management.UseCases.Mining.Wallet.Commands;
using MNX.MonitoringCenter.Management.UseCases.Mining.Wallet.Commands.AddWallet;
using MNX.MonitoringCenter.Management.UseCases.Mining.Wallet.Commands.EditWallet;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Tests;

using Algorithm = Core.Mining.Algorithm;
using Cryptocurrency = Core.Mining.Cryptocurrency;
using Wallet = Core.Mining.Wallet;

[TestFixture]
public sealed class WalletMapperTests
{
    private IWalletMapper _walletMapper;

    [SetUp]
    public void SetUp()
    {
        _walletMapper = new WalletMapper();
    }

    [Test]
    public void MapToCoreEntity_ValidAddWalletCommand_ReturnWallet()
    {
        // Arrange

        var addWalletCommand = new AddWalletCommand(
            new WalletInputModel(
                "WalletName",
                "WalletAddress",
                Guid.Parse("11111111-1111-1111-1111-111111111111")
            ),
            Guid.Parse("00000000-0000-0000-0000-000000000001")
        );

        // Act

        var mappedWallet = _walletMapper.MapToCoreEntity(addWalletCommand);

        // Assert

        Assert.Multiple(() =>
        {
            Assert.That(mappedWallet, Is.Not.Null);
            Assert.That(mappedWallet.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(mappedWallet.Name, Is.EqualTo(addWalletCommand.Model.Name));
            Assert.That(mappedWallet.Address, Is.EqualTo(addWalletCommand.Model.Address));
            Assert.That(mappedWallet.CryptocurrencyId, Is.EqualTo(addWalletCommand.Model.CryptocurrencyId));
            Assert.That(mappedWallet.Cryptocurrency, Is.Null);
            Assert.That(mappedWallet.OwnerId, Is.EqualTo(addWalletCommand.UserId));
        });
    }

    [Test]
    public void MapToCoreEntity_ValidEditWalletCommand_ReturnWallet()
    {
        // Arrange

        var editWalletCommand = new EditWalletCommand(
            Guid.Parse("00000000-0000-0000-0000-000000000001"),
            new WalletInputModel(
                "WalletName",
                "WalletAddress",
                Guid.Parse("11111111-1111-1111-1111-111111111111")
            ),
            Guid.Parse("22222222-2222-2222-2222-222222222222")
        );

        // Act

        var mappedWallet = _walletMapper.MapToCoreEntity(editWalletCommand);

        // Assert

        Assert.Multiple(() =>
        {
            Assert.That(mappedWallet, Is.Not.Null);
            Assert.That(mappedWallet.Id, Is.EqualTo(editWalletCommand.Id));
            Assert.That(mappedWallet.Name, Is.EqualTo(editWalletCommand.Model.Name));
            Assert.That(mappedWallet.Address, Is.EqualTo(editWalletCommand.Model.Address));
            Assert.That(mappedWallet.CryptocurrencyId, Is.EqualTo(editWalletCommand.Model.CryptocurrencyId));
            Assert.That(mappedWallet.Cryptocurrency, Is.Null);
            Assert.That(mappedWallet.OwnerId, Is.EqualTo(editWalletCommand.UserId));
        });
    }

    [Test]
    public void MapToModel_ValidWallet_ReturnWalletModel()
    {
        // Arrange

        var wallet = new Wallet()
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
            Name = "WalletName",
            Address = "WalletAddress",
            OwnerId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            CryptocurrencyId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            Cryptocurrency = new Cryptocurrency()
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                FullName = "Crypto",
                ShortName = "Cr",
                OwnerId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                AlgorithmId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Algorithm = new Algorithm()
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Name = "AlgorithmName",
                    OwnerId = Guid.Parse("11111111-1111-1111-1111-111111111111")
                }
            }
        };

        // Act

        var mappedModel = _walletMapper.MapToModel(wallet);

        // Assert

        Assert.Multiple(() =>
        {
            Assert.That(mappedModel, Is.Not.Null);
            Assert.That(mappedModel.Id, Is.EqualTo(wallet.Id));
            Assert.That(mappedModel.Name, Is.EqualTo(wallet.Name));
            Assert.That(mappedModel.Address, Is.EqualTo(wallet.Address));
            Assert.That(mappedModel.CryptocurrencyId, Is.EqualTo(wallet.CryptocurrencyId));
            Assert.That(mappedModel.Cryptocurrency, Is.EqualTo(wallet.Cryptocurrency.FullName));
        });
    }
}
