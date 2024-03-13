using AutoMapper;
using Kernel.UseCases;
using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;
using MNX.MonitoringCenter.Management.UseCases.Commands.Wallets;
using MNX.MonitoringCenter.Management.UseCases.Commands.Wallets.AddWallet;
using Moq;

namespace MNX.MonitoringCenter.Management.UseCases.Tests.Commands.Wallets;

[TestFixture]
public class AddWalletComandHandlerTests
{
    [Test]
    public async Task AddWallet_ReturnsWalletModel()
    {
        // Arrange
        var id = Guid.Parse("fb0637f2-5520-490f-9ba1-8591e7b72755");

        var walletRepository = new Mock<IWalletRepository>();

        walletRepository.Setup(x => x.Exists(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);

        walletRepository.Setup(x => x.Add(It.IsAny<Wallet>()));

        var cryptocurrencyRepository = new Mock<ICryptocurrencyRepository>();

        cryptocurrencyRepository.Setup(x => x.GetById(It.IsAny<Guid>()))
                                .ReturnsAsync(new Cryptocurrency());

        var mapper = new Mock<IMapper>();
        mapper.Setup(x => x.Map<Wallet>(It.IsAny<WalletInputModel>())).Returns(GetWallet());
        mapper.Setup(x => x.Map<WalletModel>(It.IsAny<Wallet>())).Returns(GetWalletModel());

        var handler = new AddWalletCommandHandler(walletRepository.Object,
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
                "Возвращенное значение не совпадает с ожидаемым идентификатором");
            Assert.That(result.Status, Is.EqualTo(ResultStatus.Created),
                "Статус результата не 'Created'");
        });

        walletRepository.Verify(x => x.Add(It.IsAny<Wallet>()), Times.Once);
    }

    [Test]
    public async Task AddWallet_WhenWalletAlreadyExists_ReturnsError()
    {
        // Arrange
        var id = Guid.Parse("fb0637f2-5520-490f-9ba1-8591e7b72755");

        var walletRepository = new Mock<IWalletRepository>();

        walletRepository.Setup(x => x.Exists(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

        walletRepository.Setup(x => x.Add(It.IsAny<Wallet>()));

        var cryptocurrencyRepository = new Mock<ICryptocurrencyRepository>();

        cryptocurrencyRepository.Setup(x => x.GetById(It.IsAny<Guid>()))
                                .ReturnsAsync(new Cryptocurrency());

        var mapper = new Mock<IMapper>();

        var handler = new AddWalletCommandHandler(walletRepository.Object,
                                                cryptocurrencyRepository.Object,
                                                mapper.Object);

        // Act
        var result = await handler.Handle(GetCommand(), default);

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
            Assert.That(result.Errors?.ElementAt(0), Is.EqualTo("Wallet already exists"),
                "Сообщение об ошибке отличается от ожидаемого");
        });

        walletRepository.Verify(x => x.Add(It.IsAny<Wallet>()), Times.Never);
    }

    [Test]
    public async Task AddWallet_WhenCryptocurrencyAlreadyExists_ReturnsError()
    {
        // Arrange
        var id = Guid.Parse("fb0637f2-5520-490f-9ba1-8591e7b72755");

        var walletRepository = new Mock<IWalletRepository>();

        walletRepository.Setup(x => x.Exists(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);

        walletRepository.Setup(x => x.Add(It.IsAny<Wallet>()));

        var cryptocurrencyRepository = new Mock<ICryptocurrencyRepository>();

        cryptocurrencyRepository.Setup(x => x.GetById(It.IsAny<Guid>()))
                                .ReturnsAsync(null as Cryptocurrency);

        var mapper = new Mock<IMapper>();

        var handler = new AddWalletCommandHandler(walletRepository.Object,
                                                  cryptocurrencyRepository.Object,
                                                  mapper.Object);

        // Act
        var result = await handler.Handle(GetCommand(), default);

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
            Assert.That(result.Errors?.ElementAt(0), Is.EqualTo("Cryptocurrency wasn't found"),
                "Сообщение об ошибке отличается от ожидаемого");
        });

        walletRepository.Verify(x => x.Add(It.IsAny<Wallet>()), Times.Never);
    }

    private static AddWalletCommand GetCommand()
    {
        return new AddWalletCommand(new WalletInputModel("Nikita", "Tomsk", Guid.Parse("f8b51c3b-d4eb-40b1-8465-4d16a79e429a")));
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
