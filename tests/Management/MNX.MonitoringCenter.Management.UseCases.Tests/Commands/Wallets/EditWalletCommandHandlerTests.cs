using AutoMapper;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;
using Moq;
using MNX.MonitoringCenter.Management.UseCases.Commands.Wallets.EditWallet;
using MNX.MonitoringCenter.Management.UseCases.Commands.Wallets;
using MNX.MonitoringCenter.Management.Core;
using Kernel.UseCases;
using MNX.MonitoringCenter.Management.Contracts;

namespace MNX.MonitoringCenter.Management.UseCases.Tests.Commands.Wallets;

[TestFixture]
public class EditWalletCommandHandlerTests
{
    [Test]
    public async Task EditWallet_ReturnsModel()
    {
        // Arrange
        var walletRepository = new Mock<IWalletRepository>();

        walletRepository.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(new Wallet());

        walletRepository.Setup(x => x.Exists(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);

        walletRepository.Setup(x => x.Update(It.IsAny<Wallet>()));

        var cryptocurrencyRepository = new Mock<ICryptocurrencyRepository>();

        cryptocurrencyRepository.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(new Cryptocurrency());

        var mapper = new Mock<IMapper>();

        mapper.Setup(x => x.Map<Wallet>(It.IsAny<WalletInputModel>())).Returns(GetWallet());
        mapper.Setup(x => x.Map<WalletModel>(It.IsAny<Wallet>())).Returns(GetWalletModel());

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
            Assert.That(result, Is.TypeOf<Result<WalletModel>>(),
                "Неверный тип результата");
            Assert.That(result.GetValue().Id, Is.EqualTo(GetWalletModel().Id),
                "Результат не совпадает с ожиданием");
            Assert.That(result.Status, Is.EqualTo(ResultStatus.Ok),
                "Статус результата не 'Ok'");
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

        cryptocurrencyRepository.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(new Cryptocurrency());

        var mapper = new Mock<IMapper>();

        mapper.Setup(x => x.Map<Wallet>(It.IsAny<WalletInputModel>())).Returns(new Wallet());

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
            Assert.That(result, Is.TypeOf<Result<WalletModel>>(),
                "Неверный тип результата");
            Assert.That(result.Status, Is.EqualTo(ResultStatus.Invalid),
                "Статус результата не 'Invalid'");
            Assert.That(result.Errors?.ElementAt(0), Is.EqualTo("Wallet with this Id wasn't found"),
                 "Сообщение об ошибке отличается от ожидаемого");
        });

        walletRepository.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);
        walletRepository.Verify(x => x.Update(It.IsAny<Wallet>()), Times.Never);
    }

    [Test]
    public async Task EditWallet_WhenWalletsAreEquals_ReturnsModel()
    {
        // Arrange

        var walletRepository = new Mock<IWalletRepository>();

        walletRepository.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(GetWallet());

        walletRepository.Setup(x => x.Exists(It.IsAny<string>(), It.IsAny<string>()));

        walletRepository.Setup(x => x.Update(It.IsAny<Wallet>()));

        var cryptocurrencyRepository = new Mock<ICryptocurrencyRepository>();
        cryptocurrencyRepository.Setup(x => x.GetById(It.IsAny<Guid>()));

        var mapper = new Mock<IMapper>();
        mapper.Setup(x => x.Map<Wallet>(It.IsAny<WalletInputModel>())).Returns(GetWallet());
        mapper.Setup(x => x.Map<WalletModel>(It.IsAny<WalletModel>())).Returns(GetWalletModel());

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
            Assert.That(result.Status, Is.EqualTo(ResultStatus.Ok),
                "Статус результата не 'Ok'");
            Assert.That(result, Is.TypeOf<Result<WalletModel>>(),
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

        cryptocurrencyRepository.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(null as Cryptocurrency);

        var mapper = new Mock<IMapper>();

        mapper.Setup(x => x.Map<Wallet>(It.IsAny<WalletInputModel>())).Returns(new Wallet());

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
            Assert.That(result, Is.TypeOf<Result<WalletModel>>(),
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

        cryptocurrencyRepository.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(null as Cryptocurrency);

        var mapper = new Mock<IMapper>();

        mapper.Setup(x => x.Map<Wallet>(It.IsAny<WalletInputModel>())).Returns(new Wallet());

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
            Assert.That(result, Is.TypeOf<Result<WalletModel>>(),
                 "Неверный тип результата");
        });

        walletRepository.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);
        walletRepository.Verify(x => x.Update(It.IsAny<Wallet>()), Times.Never);
    }

    private static EditWalletCommand GetCommand()
    {
        return new EditWalletCommand(It.IsAny<Guid>(), new WalletInputModel("Wallet", "Address", Guid.Parse("f8b51c3b-d4eb-40b1-8465-4d16a79e429a")));
    }

    private static Wallet GetWallet()
    {
        return new Wallet()
        {
            Id = Guid.Parse("f8b51c3b-d4eb-40b1-8465-4d16a79e429b"),
            Name = "Wallet",
            Address = "Address",
            CryptocurrencyId = Guid.Parse("f8b51c3b-d4eb-40b1-8465-4d16a79e429a"),
            Cryptocurrency = new Cryptocurrency()
            {
                Id = Guid.Parse("f8b51c3b-d4eb-40b1-8465-4d16a79e429a"),
                FullName = "Bitcoin",
                ShortName = "BTC",
                Algorithm = "Algorithm"
            }
        };
    }

    private static WalletModel GetWalletModel()
    {
        return new WalletModel()
        {
            Id = Guid.Parse("f8b51c3b-d4eb-40b1-8465-4d16a79e429b"),
            Name = "Wallet",
            Address = "Address",
            Cryptocurrency = "Bitcoin"
        };
    }
}
