using MNX.Application.UseCases;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Commands.Wallets.RemoveWallet;
using MNX.MonitoringCenter.Management.UseCases.Wallet;
using MNX.MonitoringCenter.Management.UseCases.Wallet.Commands.RemoveWallet;
using Moq;

namespace MNX.MonitoringCenter.Management.UseCases.Tests.Commands.Wallets.RemoveWallet;

[TestFixture]
public class RemoveWalletCommandHandlerTests
{
    [Test]
    public async Task RemoveWallet_ReturnsEmptyResult()
    {
        //Arrange
        var walletRepository = new Mock<IWalletRepository>();

        walletRepository.Setup(x => x.GetAvailableById(It.IsAny<Guid>(), TestHelper.Cryptocurrency.UserId))
                        .ReturnsAsync(new Wallet());

        walletRepository.Setup(x => x.Remove(It.IsAny<Wallet>()));

        var handler = new RemoveWalletCommandHandler(walletRepository.Object);

        // Act
        var result = await handler.Handle(GetCommand(), default);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True,
                "Операция завершилась неудачно");

            Assert.That(result.Status, Is.EqualTo(ResultStatus.NoContent),
                "Статус результата не 204");
        });

        walletRepository.Verify(x => x.GetAvailableById(It.IsAny<Guid>(), TestHelper.Cryptocurrency.UserId), Times.Once);
        walletRepository.Verify(x => x.Remove(It.IsAny<Wallet>()), Times.Once);
    }

    [Test]
    public async Task RemoveWallet_WhenWalletNull_ReturnsError()
    {
        // Arrange
        var walletRepository = new Mock<IWalletRepository>();

        walletRepository.Setup(x => x.GetAvailableById(It.IsAny<Guid>(), TestHelper.Cryptocurrency.UserId))
            .ReturnsAsync(null as Wallet);

        var handler = new RemoveWalletCommandHandler(walletRepository.Object);

        // Act
        var result = await handler.Handle(GetCommand(), default);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True,
                "Операция была неудачной, когда ожидался успех");
            Assert.That(result.Status, Is.EqualTo(ResultStatus.NoContent),
                "Статус результата не 204");
        });

        walletRepository.Verify(x => x.GetAvailableById(It.IsAny<Guid>(), TestHelper.Cryptocurrency.UserId), Times.Once);
        walletRepository.Verify(x => x.Remove(It.IsAny<Wallet>()), Times.Never);
    }

    private static RemoveWalletCommand GetCommand()
    {
        return new RemoveWalletCommand(It.IsAny<Guid>(),
            TestHelper.Cryptocurrency.UserId);
    }
}
