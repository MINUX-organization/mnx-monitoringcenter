using Kernel.UseCases;
using MediatR;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;
using MNX.MonitoringCenter.Management.UseCases.Commands.Wallets.RemoveWallet;
using Moq;

namespace MNX.MonitoringCenter.Management.UseCases.Tests.Commands.Wallets;

[TestFixture]
public class RemoveWalletCommandHandlerTests
{
    [Test]
    public async Task RemoveWallet_ReturnsEmptyResult()
    {
        //Arrange
        var walletRepository = new Mock<IWalletRepository>();

        walletRepository.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(new Wallet());

        walletRepository.Setup(x => x.Remove(It.IsAny<Wallet>()));
                            
        var handler = new RemoveWalletCommandHandler(walletRepository.Object);

        // Act
        var result = await handler.Handle(GetCommand(), default);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True,
                "Операция завершилась неудачно");
            Assert.That(result.Errors, Is.Null,
                "Список ошибок не пуст");
            Assert.That(result, Is.EqualTo(Result<Unit>.Empty()),
                "Результат не пуст");
            Assert.That(result, Is.TypeOf<Result<Unit>>(),
                "Неверный тип результата");
            Assert.That(result.Status, Is.EqualTo(ResultStatus.NoContent),
                "Статус результата не 'NoContent'");
        });

        walletRepository.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);
        walletRepository.Verify(x => x.Remove(It.IsAny<Wallet>()), Times.Once);
    }

    [Test]
    public async Task RemoveWallet_WhenWalletNull_ReturnsError()
    {
        // Arrange
        var walletRepository = new Mock<IWalletRepository>();

        walletRepository.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(null as Wallet);

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
            Assert.That(result, Is.TypeOf<Result<Unit>>(),
                "Неверный тип результата");
            Assert.That(result, Is.Not.EqualTo(Result<Unit>.Empty()),
                "Результат пуст");
            Assert.That(result.Status, Is.EqualTo(ResultStatus.Invalid),
                "Статус результата не 'Invalid'");
            Assert.That(result.Errors?.ElementAt(0), Is.EqualTo("Wallet with this Id wasn't found"),
                "Сообщение об ошибке отличается от ожидаемого");
        });

        walletRepository.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);
        walletRepository.Verify(x => x.Remove(It.IsAny<Wallet>()), Times.Never);
    }

    private static RemoveWalletCommand GetCommand()
    {
        return new RemoveWalletCommand(It.IsAny<Guid>());
    }
}
