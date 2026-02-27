using MNX.MonitoringCenter.Management.Tests.Service.Builders.ContractBuilders;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Wallet;
using MNX.MonitoringCenter.Management.UseCases.Mining.Wallet;
using MNX.MonitoringCenter.Management.UseCases.Mining.Wallet.Commands.AddWallet;
using MNX.MonitoringCenter.Management.UseCases.Mining.Wallet.Commands.EditWallet;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Tests;

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
            new WalletInputModelBuilder().Build(),
            Guid.NewGuid()
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

        CryptocurrencyBuilder CreateCrypto()
        {
            return new CryptocurrencyBuilder()
                .WithAlgorithm(algo => algo.WithName("algo1"));
        }
    }

    [Test]
    public void MapToCoreEntity_ValidEditWalletCommand_ReturnWallet()
    {
        // Arrange

        var editWalletCommand = new EditWalletCommand(
            Guid.NewGuid(),
            new WalletInputModelBuilder().Build(),
            Guid.NewGuid()
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
        var wallet = new WalletBuilder()
            .WithCryptocurrency(crypto =>
                crypto.WithOwner()
                      .WithAlgorithm(algo =>
                        algo.WithOwner()))
            .Build();
        

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
