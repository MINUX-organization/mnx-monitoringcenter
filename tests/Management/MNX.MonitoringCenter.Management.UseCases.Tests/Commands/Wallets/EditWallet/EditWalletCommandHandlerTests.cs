using AutoMapper;
using Moq;
using MNX.MonitoringCenter.Management.UseCases.Commands.Wallets.EditWallet;
using MNX.MonitoringCenter.Management.UseCases.Commands.Wallets;
using MNX.MonitoringCenter.Management.Core;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Management.UseCases.Cryptocurrency;
using MNX.MonitoringCenter.Management.UseCases.Wallet;
using MNX.MonitoringCenter.Management.UseCases.Wallet.Commands.EditWallet;

namespace MNX.MonitoringCenter.Management.UseCases.Tests.Commands.Wallets.EditWallet;

[TestFixture]
public class EditWalletCommandHandlerTests
{
    private IMapper _mapper;

    private static readonly Guid _walletId = Guid.Parse("f8b51c3b-d4eb-40b1-8465-4d16a79e429b");

    [SetUp]
    public void Setup()
    {
        _mapper = TestHelper.GetMapper();
    }

    [Test]
    public async Task EditWallet_ReturnsModel()
    {
        // Arrange
        var walletRepository = new Mock<IWalletRepository>();

        walletRepository.Setup(x => x.GetAvailableById(It.IsAny<Guid>(), TestHelper.UserId)).ReturnsAsync(GetWallet());

        walletRepository.Setup(x => x.Exists(TestHelper.UserId, It.IsAny<string>(), It.IsAny<string>()))
                        .ReturnsAsync(false);

        walletRepository.Setup(x => x.Update(It.IsAny<Wallet>()));

        var cryptocurrencyRepository = new Mock<ICryptocurrencyRepository>();

        cryptocurrencyRepository.Setup(x => x.GetAvailableById(It.IsAny<Guid>(), TestHelper.UserId))
                                .ReturnsAsync(TestHelper.Cryptocurrency);

        var handler = new EditWalletCommandHandler(walletRepository.Object,
                                                   cryptocurrencyRepository.Object,
                                                   _mapper);

        // Act
        var result = await handler.Handle(GetCommand("new_wallet", "new_address", TestHelper.Cryptocurrency.Id), default);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True, "Операция завершилась неудачно");

            Assert.That(result.Status, Is.EqualTo(ResultStatus.Ok), "Статус результата не 'Ok'");

            Assert.That(result.Errors, Is.Null, "Список ошибок не пуст");

            Assert.That(result.GetValue().Id == _walletId &&
                        result.GetValue().Name == "new_wallet" &&
                        result.GetValue().Address == "new_address" &&
                        result.GetValue().Cryptocurrency == TestHelper.Cryptocurrency.FullName,
                "Результат не совпадает с ожиданием");

        });

        walletRepository.Verify(x => x.GetAvailableById(It.IsAny<Guid>(), TestHelper.UserId), Times.Once);
        walletRepository.Verify(x => x.Update(It.IsAny<Wallet>()), Times.Once);
    }

    [Test]
    public async Task EditWallet_WhenWalletIsNull_ReturnsError()
    {
        // Arrange
        var walletRepository = new Mock<IWalletRepository>();

        walletRepository.Setup(x => x.GetAvailableById(It.IsAny<Guid>(), TestHelper.UserId))
            .ReturnsAsync(null as Wallet);

        walletRepository.Setup(x => x.Exists(TestHelper.UserId, It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);

        walletRepository.Setup(x => x.Update(It.IsAny<Wallet>()));

        var cryptocurrencyRepository = new Mock<ICryptocurrencyRepository>();

        cryptocurrencyRepository.Setup(x => x.GetAvailableById(It.IsAny<Guid>(), TestHelper.UserId))
                                .ReturnsAsync(TestHelper.Cryptocurrency);

        var handler = new EditWalletCommandHandler(walletRepository.Object,
                                                   cryptocurrencyRepository.Object,
                                                   _mapper);

        // Act
        var result = await handler.Handle(GetCommand("new_wallet", "new_address", TestHelper.Cryptocurrency.Id), default);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.False,
                "Операция завершилась успешно, когда ожидалась неудача");
            Assert.That(result.Errors, Is.Not.Null,
                "Список ошибок пуст");
            Assert.That(result.Status, Is.EqualTo(ResultStatus.Invalid),
                "Статус результата не 400");
            Assert.That(result.Errors?.ElementAt(0), Is.EqualTo("Wallet with this Id wasn't found"),
                 "Сообщение об ошибке отличается от ожидаемого");
        });

        walletRepository.Verify(x => x.GetAvailableById(It.IsAny<Guid>(), TestHelper.UserId), Times.Once);
        walletRepository.Verify(x => x.Update(It.IsAny<Wallet>()), Times.Never);
    }

    [Test]
    public async Task EditWallet_WhenWalletAreEquals_ReturnsModel()
    {
        // Arrange

        var walletRepository = new Mock<IWalletRepository>();

        walletRepository.Setup(x => x.GetAvailableById(It.IsAny<Guid>(), TestHelper.UserId)).ReturnsAsync(GetWallet());

        walletRepository.Setup(x => x.Exists(TestHelper.UserId, It.IsAny<string>(), It.IsAny<string>()));

        walletRepository.Setup(x => x.Update(It.IsAny<Wallet>()));

        var cryptocurrencyRepository = new Mock<ICryptocurrencyRepository>();
        cryptocurrencyRepository.Setup(x => x.GetAvailableById(It.IsAny<Guid>(), TestHelper.UserId));

        var handler = new EditWalletCommandHandler(walletRepository.Object,
                                                   cryptocurrencyRepository.Object,
                                                   _mapper);

        // Act
        var result = await handler.Handle(GetCommand("wallet", "address", TestHelper.Cryptocurrency.Id), default);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True,
                 "Операция завершилась неудачно");
            Assert.That(result.Errors, Is.Null,
                "Список не ошибок пуст");
            Assert.That(result.Status, Is.EqualTo(ResultStatus.Ok),
                "Статус результата не 'Ok'");

            Assert.That(result.GetValue().Id == _walletId &&
                        result.GetValue().Name == "wallet" &&
                        result.GetValue().Address == "address" &&
                        result.GetValue().Cryptocurrency == TestHelper.Cryptocurrency.FullName,
                "Результат не совпадает с ожиданием");
        });

        walletRepository.Verify(x => x.GetAvailableById(It.IsAny<Guid>(), TestHelper.UserId), Times.Once);
        walletRepository.Verify(x => x.Update(It.IsAny<Wallet>()), Times.Never);
    }

    [Test]
    public async Task EditWallet_WhenWalletAlreadyExists_ReturnsError()
    {
        // Arrange
        var walletRepository = new Mock<IWalletRepository>();

        walletRepository.Setup(x => x.GetAvailableById(It.IsAny<Guid>(), TestHelper.UserId))
            .ReturnsAsync(new Wallet());

        walletRepository.Setup(x => x.Exists(TestHelper.UserId, It.IsAny<string>(), It.IsAny<string>(), _walletId))
                        .ReturnsAsync(true);

        walletRepository.Setup(x => x.Update(It.IsAny<Wallet>()));

        var cryptocurrencyRepository = new Mock<ICryptocurrencyRepository>();

        cryptocurrencyRepository.Setup(x => x.GetAvailableById(It.IsAny<Guid>(), TestHelper.UserId))
                                .ReturnsAsync(null as Cryptocurrency);

        var handler = new EditWalletCommandHandler(walletRepository.Object,
                                                   cryptocurrencyRepository.Object,
                                                   _mapper);

        // Act
        var result = await handler.Handle(GetCommand("wallet", "address", TestHelper.Cryptocurrency.Id), default);

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
                "Статус результата не 400");
        });

        walletRepository.Verify(x => x.GetAvailableById(It.IsAny<Guid>(), TestHelper.UserId), Times.Once);
        walletRepository.Verify(x => x.Update(It.IsAny<Wallet>()), Times.Never);
    }

    [Test]
    public async Task EditWallet_WhenCryptocurrencyDoesNotExist_ReturnsError()
    {
        // Arrange
        var walletRepository = new Mock<IWalletRepository>();

        walletRepository.Setup(x => x.GetAvailableById(It.IsAny<Guid>(), TestHelper.UserId))
                        .ReturnsAsync(GetWallet());

        walletRepository.Setup(x => x.Exists(TestHelper.UserId, It.IsAny<string>(), It.IsAny<string>()))
                        .ReturnsAsync(false);

        walletRepository.Setup(x => x.Update(It.IsAny<Wallet>()));

        var cryptocurrencyRepository = new Mock<ICryptocurrencyRepository>();

        cryptocurrencyRepository.Setup(x => x.GetAvailableById(It.IsAny<Guid>(), TestHelper.Cryptocurrency.UserId))
                                .ReturnsAsync(null as Cryptocurrency);

        var handler = new EditWalletCommandHandler(walletRepository.Object,
                                                   cryptocurrencyRepository.Object,
                                                   _mapper);

        // Act
        var result = await handler.Handle
            (GetCommand("new_wallet", "new_address", TestHelper.Cryptocurrency.Id), default);

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
                "Статус результата не 400");
        });

        walletRepository.Verify(x => x.GetAvailableById(It.IsAny<Guid>(), TestHelper.UserId), Times.Once);
        walletRepository.Verify(x => x.Update(It.IsAny<Wallet>()), Times.Never);
    }

    private static EditWalletCommand GetCommand(string name, string address, Guid cryptoId)
    {
        return new EditWalletCommand(_walletId, new WalletInputModel(name, address, cryptoId),
            TestHelper.Cryptocurrency.UserId);
    }

    private static Wallet GetWallet()
    {
        return new Wallet()
        {
            Id = _walletId,
            Name = "wallet",
            Address = "address",
            CryptocurrencyId = TestHelper.Cryptocurrency.Id,
            Cryptocurrency = TestHelper.Cryptocurrency
        };
    }
}
