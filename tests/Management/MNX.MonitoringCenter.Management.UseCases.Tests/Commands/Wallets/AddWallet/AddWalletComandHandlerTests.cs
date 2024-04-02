using AutoMapper;
using Kernel.UseCases;
using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;
using MNX.MonitoringCenter.Management.UseCases.Commands.Wallets;
using MNX.MonitoringCenter.Management.UseCases.Commands.Wallets.AddWallet;
using Moq;

namespace MNX.MonitoringCenter.Management.UseCases.Tests.Commands.Wallets.AddWallet;

[TestFixture]
public class AddWalletComandHandlerTests
{
    private IMapper _mapper;

    [SetUp]
    public void Setup()
    {
        _mapper = TestHelper.GetMapper();
    }

    [Test]
    public async Task AddWallet_ReturnsWalletModel()
    {
        // Arrange
        var walletRepository = new Mock<IWalletRepository>();

        walletRepository.Setup(x => x.Exists(TestHelper.UserId, It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);

        walletRepository.Setup(x => x.Add(It.IsAny<Wallet>()))
                        .ReturnsAsync(Guid.Parse("f8b51c3b-d4eb-40b1-8465-4d16a79e429b"));

        var cryptocurrencyRepository = new Mock<ICryptocurrencyRepository>();

        cryptocurrencyRepository.Setup(x => x.GetAvailableById(It.IsAny<Guid>(), TestHelper.UserId))
                                .ReturnsAsync(TestHelper.Cryptocurrency);

        var handler = new AddWalletCommandHandler(walletRepository.Object,
                                                  cryptocurrencyRepository.Object,
                                                  _mapper);

        // Act
        var result = await handler.Handle(GetCommand(), default);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True, "Операция завершилась неудачно");

            Assert.That(result.Status, Is.EqualTo(ResultStatus.Created), "Статус результата не 201");

            Assert.IsTrue(result.GetValue().Id == GetWalletModel().Id &&
                          result.GetValue().Name == GetWalletModel().Name &&
                          result.GetValue().Address == GetWalletModel().Address &&
                          result.GetValue().Cryptocurrency == GetWalletModel().Cryptocurrency,
                    "Возвращенное значение не совпадает с ожидаемым");
        });

        walletRepository.Verify(x => x.Add(It.IsAny<Wallet>()), Times.Once);
    }

    [Test]
    public async Task AddWallet_WhenWalletAlreadyExists_ReturnsError()
    {
        // Arrange
        var walletRepository = new Mock<IWalletRepository>();

        walletRepository.Setup(x => x.Exists(TestHelper.UserId, It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

        var cryptocurrencyRepository = new Mock<ICryptocurrencyRepository>();

        cryptocurrencyRepository.Setup(x => x.GetAvailableById(It.IsAny<Guid>(), TestHelper.Cryptocurrency.UserId))
                                .ReturnsAsync(TestHelper.Cryptocurrency);

        var handler = new AddWalletCommandHandler(walletRepository.Object,
                                                  cryptocurrencyRepository.Object,
                                                  _mapper);

        // Act
        var result = await handler.Handle(GetCommand(), default);

        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.False,
                "Операция завершилась успешно, когда ожидалась неудача");
            Assert.That(result.Errors, Is.Not.Null,
                "Список ошибок пуст");
            Assert.That(result.Status, Is.EqualTo(ResultStatus.Invalid),
                "Статус результата не 400");
            Assert.That(result.Errors?.ElementAt(0), Is.EqualTo("Wallet already exists"),
                "Сообщение об ошибке отличается от ожидаемого");
        });

        walletRepository.Verify(x => x.Add(It.IsAny<Wallet>()), Times.Never);
    }

    [Test]
    public async Task AddWallet_WhenCryptocurrencyAlreadyExists_ReturnsError()
    {
        // Arrange
        var walletRepository = new Mock<IWalletRepository>();

        walletRepository.Setup(x => x.Exists(TestHelper.UserId, It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);

        walletRepository.Setup(x => x.Add(It.IsAny<Wallet>()));

        var cryptocurrencyRepository = new Mock<ICryptocurrencyRepository>();

        cryptocurrencyRepository.Setup(x => x.GetAvailableById(It.IsAny<Guid>(), TestHelper.Cryptocurrency.UserId))
                                .ReturnsAsync(null as Cryptocurrency);

        var handler = new AddWalletCommandHandler(walletRepository.Object,
                                                  cryptocurrencyRepository.Object,
                                                  _mapper);

        // Act
        var result = await handler.Handle(GetCommand(), default);

        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.False,
                "Операция завершилась успешно, когда ожидалась неудача");
            Assert.That(result.Errors, Is.Not.Null,
                "Список ошибок пуст");
            Assert.That(result.Status, Is.EqualTo(ResultStatus.Invalid),
                "Статус результата не 400");
            Assert.That(result.Errors?.ElementAt(0), Is.EqualTo("Cryptocurrency wasn't found"),
                "Сообщение об ошибке отличается от ожидаемого");
        });

        walletRepository.Verify(x => x.Add(It.IsAny<Wallet>()), Times.Never);
    }

    private static AddWalletCommand GetCommand()
    {
        return new AddWalletCommand
            (new WalletInputModel("Wallet", "Address",
            Guid.Parse("f8b51c3b-d4eb-40b1-8465-4d16a79e429a")),
            TestHelper.Cryptocurrency.UserId);
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
