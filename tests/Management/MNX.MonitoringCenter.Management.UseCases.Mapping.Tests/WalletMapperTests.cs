using FluentAssertions;
using MNX.MonitoringCenter.Management.Contracts;
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

        mappedWallet.Should().Satisfy<Core.Mining.Wallet>(x =>
        {
            x.Id.Should().NotBe(Guid.Empty);
            x.Name.Should().Be(addWalletCommand.Model.Name);
            x.Address.Should().Be(addWalletCommand.Model.Address);
            x.CryptocurrencyId.Should().Be(addWalletCommand.Model.CryptocurrencyId);
            x.OwnerId.Should().Be(addWalletCommand.UserId);
            x.Cryptocurrency.Should().BeNull();
        });
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

        mappedWallet.Should().Satisfy<Core.Mining.Wallet>(x =>
        {
            x.Id.Should().Be(editWalletCommand.Id);
            x.Name.Should().Be(editWalletCommand.Model.Name);
            x.Address.Should().Be(editWalletCommand.Model.Address);
            x.CryptocurrencyId.Should().Be(editWalletCommand.Model.CryptocurrencyId);
            x.Cryptocurrency.Should().BeNull();
            x.OwnerId.Should().Be(editWalletCommand.UserId);
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

        mappedModel.Should().Satisfy<WalletModel>(x =>
        {
            x.Id.Should().Be(wallet.Id);
            x.Name.Should().Be(wallet.Name);
            x.Address.Should().Be(wallet.Address);
            x.CryptocurrencyId.Should().Be(wallet.CryptocurrencyId);
            x.Cryptocurrency.Should().Be(wallet.Cryptocurrency!.FullName);
        });
    }
}
