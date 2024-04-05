using MNX.Application.UseCases;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;
using MNX.MonitoringCenter.Management.UseCases.Commands.Wallets.RemoveWallet;
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


    private static RemoveWalletCommand GetCommand()
    {
        return new RemoveWalletCommand(It.IsAny<Guid>(),
            TestHelper.Cryptocurrency.UserId);
    }
}
