using MNX.MonitoringCenter.Management.Tests.Service.Assertions;
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

        mappedWallet.ShouldBeEqualTo(addWalletCommand);
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

        mappedWallet.ShouldBeEqualTo(editWalletCommand);
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

        mappedModel.ShouldBeEqualTo(wallet);
    }
}
