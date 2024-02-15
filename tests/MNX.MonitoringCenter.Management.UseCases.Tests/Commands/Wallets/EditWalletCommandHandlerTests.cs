using AutoMapper;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;
using Moq;
using MNX.MonitoringCenter.Management.UseCases.Commands.Wallets.EditWallet;
using MNX.MonitoringCenter.Management.UseCases.Commands.Wallets;
using MNX.MonitoringCenter.Management.Core;
using Kernel.UseCases;
using MediatR;

namespace MNX.MonitoringCenter.Management.UseCases.Tests.Commands.Wallets;

[TestFixture]
public class EditWalletCommandHandlerTests
{
    [Test]
    public async Task EditWallet_ReturnsEmptyResult()
    {
        // Arrange
        var walletRepository = new Mock<IWalletRepository>();

        walletRepository.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(new Wallet());

        walletRepository.Setup(x => x.Exists(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);

        walletRepository.Setup(x => x.Update(It.IsAny<Wallet>()));

        var cryptocurrencyRepository = new Mock<ICryptocurrencyRepository>();

        cryptocurrencyRepository.Setup(x => x.Exists(It.IsAny<string>(), default)).ReturnsAsync(true);

        var mapper = new Mock<IMapper>();

        mapper.Setup(x => x.Map<Wallet>(It.IsAny<WalletModel>())).Returns(new Wallet());

        var handler = new EditWalletCommandHandler(walletRepository.Object,
                                                cryptocurrencyRepository.Object,
                                                mapper.Object);

        // Act
        var result = await handler.Handle(GetCommand(), default);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True,
                "Операция завершилась неудачно");
            Assert.That(result.Errors, Is.Null,
                "Список ошибок не пуст");
            Assert.That(result, Is.TypeOf<Result<Unit>>(),
                "Неверный тип результата");
            Assert.That(result, Is.EqualTo(Result<Unit>.Empty()),
                "Результат не пуст");
            Assert.That(result.Status, Is.EqualTo(ResultStatus.NoContent),
                "Статус результата не 'NoContent'");
        });

        walletRepository.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);
        walletRepository.Verify(x => x.Update(It.IsAny<Wallet>()), Times.Once);
    }

    [Test]
    public async Task EditWallet_WhenWalletIsNull_ReturnsError()
    {
        // Arrange
        var walletRepository = new Mock<IWalletRepository>();

        walletRepository.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(null as Wallet);

        walletRepository.Setup(x => x.Exists(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);

        walletRepository.Setup(x => x.Update(It.IsAny<Wallet>()));

        var cryptocurrencyRepository = new Mock<ICryptocurrencyRepository>();

        cryptocurrencyRepository.Setup(x => x.Exists(It.IsAny<string>(), default)).ReturnsAsync(true);

        var mapper = new Mock<IMapper>();

        mapper.Setup(x => x.Map<Wallet>(It.IsAny<WalletModel>())).Returns(new Wallet());

        var handler = new EditWalletCommandHandler(walletRepository.Object,
                                                cryptocurrencyRepository.Object,
                                                mapper.Object);

        // Act
        var result = await handler.Handle(GetCommand(), default);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.False,
                "Операция завершилась успешно, когда ожидалась неудача");
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
        walletRepository.Verify(x => x.Update(It.IsAny<Wallet>()), Times.Never);
    }

    [Test]
    public async Task EditWallet_WhenWalletsAreEquals_ReturnsEmptyResult()
    {
        // Arrange
        var wallet = new Wallet()
        {
            Id = Guid.NewGuid(),
            Name = "Nikita",
            Address = "Tomsk",
            Cryptocurrency = "Bitcoin"
        };

        var walletRepository = new Mock<IWalletRepository>();

        walletRepository.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(wallet);

        walletRepository.Setup(x => x.Exists(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);

        walletRepository.Setup(x => x.Update(It.IsAny<Wallet>()));

        var cryptocurrencyRepository = new Mock<ICryptocurrencyRepository>();
        cryptocurrencyRepository.Setup(x => x.Exists(It.IsAny<string>(), default)).ReturnsAsync(true);

        var mapper = new Mock<IMapper>();
        mapper.Setup(x => x.Map<Wallet>(It.IsAny<WalletModel>())).Returns(new Wallet());

        var handler = new EditWalletCommandHandler(walletRepository.Object,
                                                cryptocurrencyRepository.Object,
                                                mapper.Object);

        // Act
        var result = await handler.Handle(GetCommand(), default);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True,
                 "Операция завершилась неудачно");
            Assert.That(result.Errors, Is.Null,
                "Список не ошибок пуст");
            Assert.That(result.Status, Is.EqualTo(ResultStatus.NoContent),
                "Статус результата не 'NoContent'");
            Assert.That(result, Is.EqualTo(Result<Unit>.Empty()),
                "Результат не пуст");
            Assert.That(result, Is.TypeOf<Result<Unit>>(),
                 "Неверный тип результата");
        });

        walletRepository.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);
        walletRepository.Verify(x => x.Update(It.IsAny<Wallet>()), Times.Never);
    }

    [Test]
    public async Task EditWallet_WhenWalletAlreadyExists_ReturnsError()
    {
        // Arrange
        var walletRepository = new Mock<IWalletRepository>();

        walletRepository.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(new Wallet());

        walletRepository.Setup(x => x.Exists(It.IsAny<string>(), It.IsAny<string>(), default)).ReturnsAsync(true);

        walletRepository.Setup(x => x.Update(It.IsAny<Wallet>()));

        var cryptocurrencyRepository = new Mock<ICryptocurrencyRepository>();

        cryptocurrencyRepository.Setup(x => x.Exists(It.IsAny<string>(), default)).ReturnsAsync(false);

        var mapper = new Mock<IMapper>();

        mapper.Setup(x => x.Map<Wallet>(It.IsAny<WalletModel>())).Returns(new Wallet());

        var handler = new EditWalletCommandHandler(walletRepository.Object,
                                                cryptocurrencyRepository.Object,
                                                mapper.Object);

        // Act
        var result = await handler.Handle(GetCommand(), default);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.False,
                "Операция завершилась успешно, когда ожидалась неудача");
            Assert.That(result.Errors, Is.Not.Null,
                "Список ошибок пуст");
            Assert.That(result.Errors?.ElementAt(0), Is.EqualTo("Wallet with this name or address already exists"),
                 "Сообщение об ошибке отличается от ожидаемого");
            Assert.That(result.Status, Is.EqualTo(ResultStatus.Invalid),
                "Статус результата не 'Invalid'");
            Assert.That(result, Is.TypeOf<Result<Unit>>(),
                 "Неверный тип результата");
        });

        walletRepository.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);
        walletRepository.Verify(x => x.Update(It.IsAny<Wallet>()), Times.Never);
    }

    [Test]
    public async Task EditWallet_WhenCryptocurrencyDoesNotExist_ReturnsError()
    {
        // Arrange
        var walletRepository = new Mock<IWalletRepository>();

        walletRepository.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(new Wallet());

        walletRepository.Setup(x => x.Exists(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);

        walletRepository.Setup(x => x.Update(It.IsAny<Wallet>()));

        var cryptocurrencyRepository = new Mock<ICryptocurrencyRepository>();

        cryptocurrencyRepository.Setup(x => x.Exists(It.IsAny<string>(), default)).ReturnsAsync(false);

        var mapper = new Mock<IMapper>();

        mapper.Setup(x => x.Map<Wallet>(It.IsAny<WalletModel>())).Returns(new Wallet());

        var handler = new EditWalletCommandHandler(walletRepository.Object,
                                                cryptocurrencyRepository.Object,
                                                mapper.Object);

        // Act
        var result = await handler.Handle(GetCommand(), default);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.False,
                "Операция завершилась успешно, когда ожидалась неудача");
            Assert.That(result.Errors, Is.Not.Null,
                "Список ошибок пуст");
            Assert.That(result.Errors?.ElementAt(0), Is.EqualTo("Cryptocurrency wasn't found"),
                 "Сообщение об ошибке отличается от ожидаемого");
            Assert.That(result.Status, Is.EqualTo(ResultStatus.Invalid),
                "Статус результата не 'Invalid'");
            Assert.That(result, Is.TypeOf<Result<Unit>>(),
                 "Неверный тип результата");
        });

        walletRepository.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);
        walletRepository.Verify(x => x.Update(It.IsAny<Wallet>()), Times.Never);
    }

    private static EditWalletCommand GetCommand()
    {
        return new EditWalletCommand(It.IsAny<Guid>(), new WalletModel("Nikita", "Tomsk", "Bitcoin"));
    }
}
