using AutoMapper;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;
using MNX.MonitoringCenter.Management.UseCases.Commands.Crypto.AddCryptocurrency;
using Moq;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Commands.Crypto;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Management.Contracts;

namespace MNX.MonitoringCenter.Management.UseCases.Tests.Commands.Crypto.AddCryptocurrency
{
    [TestFixture]
    public class AddCryptocurrencyCommandHandlerTests
    {
        private Mock<ICryptocurrencyRepository> _cryptoRepository;

        private Mock<IAlgorithmRepository> _algorithmRepository;

        private IMapper _mapper = TestHelper.GetMapper();

        private AddCryptocurrencyCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _cryptoRepository = new Mock<ICryptocurrencyRepository>();
            _algorithmRepository = new Mock<IAlgorithmRepository>();

            _handler = new AddCryptocurrencyCommandHandler(
                _cryptoRepository.Object,
                _algorithmRepository.Object,
                _mapper);
        }

        [Test]
        public async Task AddCrypto_ReturnsCryptoModel()
        {
            var cryptocurrency = new Cryptocurrency()
            {
                Id = Guid.Parse("f8b51c3b-d4eb-40b1-8465-4d16a79e429a"),
                FullName = "Test",
                ShortName = "Test",
                Algorithm = "Test"
            };

            _cryptoRepository
                .Setup(x => x.Exists(TestHelper.Cryptocurrency.UserId, It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            _algorithmRepository
                .Setup(x => x.Exists(It.IsAny<string>()))
                .ReturnsAsync(true);

            var result = await _handler.Handle(GetCommand(), default);

            _cryptoRepository
                .Verify(x => x.Add(It.IsAny<Cryptocurrency>()), Times.Once);

            Assert.NotNull(result);
            Assert.IsTrue(result.IsSuccess);
            Assert.IsTrue(result.GetValue().ShortName == GetCommand().Model.ShortName &&
                          result.GetValue().FullName == GetCommand().Model.FullName &&
                          result.GetValue().Algorithm == GetCommand().Model.Algorithm,
                          "Возвращаемое значение не совпадает с ожидаемым");
        }

        [Test]
        public async Task AddCrypto_WhenCryptoAlreadyExists_ReturnsError()
        {
            _cryptoRepository
                .Setup(x => x.Exists(TestHelper.Cryptocurrency.UserId, It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            var result = await _handler.Handle(GetCommand(), default);

            _cryptoRepository
                .Verify(x => x.Add(It.IsAny<Cryptocurrency>()), Times.Never);

            Assert.NotNull(result);
            Assert.IsFalse(result.IsSuccess);
            Assert.NotNull(result.Errors);
            Assert.That(result.Status, Is.EqualTo(ResultStatus.Conflict));
            Assert.That(result.Errors?.ElementAt(0),
                Is.EqualTo("Cryptocurrency already exists"));
        }

        [Test]
        public async Task AddCrypto_WhenAlgorithmDoesNotExist_ReturnsError()
        {
            _cryptoRepository
                .Setup(x => x.Exists(TestHelper.Cryptocurrency.UserId, It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            _algorithmRepository
                .Setup(x => x.Exists(It.IsAny<string>()))
                .ReturnsAsync(false);

            var result = await _handler.Handle(GetCommand(), default);

            _cryptoRepository
                .Verify(x => x.Add(It.IsAny<Cryptocurrency>()), Times.Never);

            Assert.NotNull(result);
            Assert.IsFalse(result.IsSuccess);
            Assert.NotNull(result.Errors);
            Assert.That(result.Status, Is.EqualTo(ResultStatus.Invalid));
            Assert.That(result.Errors?.ElementAt(0),
                Is.EqualTo("Algorithm wasn't found"));
        }

        private static AddCryptocurrencyCommand GetCommand()
        {
            return new AddCryptocurrencyCommand
                (new CryptocurrencyInputModel("BTC", "Bitcoin", "SHA-256"), TestHelper.Cryptocurrency.UserId);
        }
    }
}
