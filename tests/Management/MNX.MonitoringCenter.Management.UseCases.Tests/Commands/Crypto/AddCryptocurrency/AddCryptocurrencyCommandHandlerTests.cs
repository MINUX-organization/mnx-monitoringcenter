using AutoMapper;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;
using MNX.MonitoringCenter.Management.UseCases.Commands.Crypto.AddCryptocurrency;
using Moq;
using Kernel.UseCases;
using MNX.MonitoringCenter.Management.Core;

namespace MNX.MonitoringCenter.Management.UseCases.Tests.Commands.Crypto.AddCryptocurrency
{
    [TestFixture]
    public class AddCryptocurrencyCommandHandlerTests
    {
        private Mock<ICryptocurrencyRepository> _cryptoRepository;

        private Mock<IAlgorithmRepository> _algorithmRepository;

        private Mock<IMapper> _mapper;

        private AddCryptocurrencyCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _cryptoRepository = new Mock<ICryptocurrencyRepository>();
            _algorithmRepository = new Mock<IAlgorithmRepository>();
            _mapper = new Mock<IMapper>();

            _handler = new AddCryptocurrencyCommandHandler(
                _cryptoRepository.Object,
                _algorithmRepository.Object,
                _mapper.Object);
        }

        [Test]
        public async Task AddCrypto_ReturnsUnitValue()
        {
            var cryptocurrency = new Cryptocurrency()
            {
                Id = 1,
                FullName = "Test",
                ShortName = "Test",
                AlgorithmName = "Test"
            };

            _cryptoRepository
                .Setup(x => x.Exists(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            _algorithmRepository
                .Setup(x => x.Exists(It.IsAny<string>()))
                .ReturnsAsync(true);

            _mapper
                .Setup(x => x.Map<Cryptocurrency>(
                    It.IsAny<AddCryptocurrencyCommand>()))
                .Returns(cryptocurrency);

            var result = await _handler.Handle(GetCommand(), default);

            _cryptoRepository
                .Verify(x => x.Add(It.IsAny<Cryptocurrency>()), Times.Once);

            Assert.NotNull(result);
            Assert.IsTrue(result.IsSuccess);
            Assert.That(result.GetValue(), Is.EqualTo(cryptocurrency));
        }

        [Test]
        public async Task AddCrypto_WhenCryptoAlreadyExists_ReturnsError()
        {
            _cryptoRepository
                .Setup(x => x.Exists(It.IsAny<string>(), It.IsAny<string>()))
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
                .Setup(x => x.Exists(It.IsAny<string>(), It.IsAny<string>()))
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
            return new AddCryptocurrencyCommand("BTC", "Bitcoin", "SHA-256");
        }
    }
}
