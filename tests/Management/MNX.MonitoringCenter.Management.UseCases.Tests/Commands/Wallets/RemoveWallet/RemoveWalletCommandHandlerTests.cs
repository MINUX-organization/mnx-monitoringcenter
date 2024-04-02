using Kernel.UseCases;
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
            Assert.That(result.IsSuccess, Is.False,
                "Операция была успешной, когда ожидалась неудача");
            Assert.That(result.Errors, Is.Not.Null,
                "Список ошибок пуст");
            Assert.That(result.Status, Is.EqualTo(ResultStatus.Invalid),
                "Статус результата не 400");
            Assert.That(result.Errors?.ElementAt(0), Is.EqualTo("Wallet with this Id wasn't found"),
                "Сообщение об ошибке отличается от ожидаемого");
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
