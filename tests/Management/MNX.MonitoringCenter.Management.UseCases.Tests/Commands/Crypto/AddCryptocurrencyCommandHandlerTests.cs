using AutoMapper;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;
using MNX.MonitoringCenter.Management.UseCases.Commands.Crypto.AddCryptocurrency;
using Moq;
using Kernel.UseCases;
using MediatR;
using MNX.MonitoringCenter.Management.Core;

namespace MNX.MonitoringCenter.Management.UseCases.Tests.Commands.Crypto
{
    public class AddCryptocurrencyCommandHandlerTests
    {
        [Test]
        public async Task AddCrypto_ReturnsUnitValue()
        {
            var cryptoRepository = new Mock<ICryptocurrencyRepository>();
            cryptoRepository
                .Setup(x=>x.Exists(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            var algorithmRepository = new Mock<IAlgorithmRepository>();
            algorithmRepository
                .Setup(x=>x.Exists(It.IsAny<string>()))
                .ReturnsAsync(true);

            var mapper = new Mock<IMapper>();
            mapper
                .Setup(x=>x.Map<Cryptocurrency>(
                    It.IsAny<AddCryptocurrencyCommand>()))
                .Returns(new Cryptocurrency());

            var handler = new AddCryptocurrencyCommandHandler(
                cryptoRepository.Object,
                algorithmRepository.Object,
                mapper.Object);

            var result = await handler.Handle(GetCommand(), default);

            cryptoRepository
                .Verify(x => x.Add(It.IsAny<Cryptocurrency>()), Times.Once);

            Assert.NotNull(result);
            Assert.IsTrue(result.IsSuccess);
            Assert.That(result.GetValue(), Is.EqualTo(Unit.Value));
        }

        [Test]
        public async Task AddCrypto_WhenCryptoAlreadyExists_ReturnsError()
        {
            var cryptoRepository = new Mock<ICryptocurrencyRepository>();
            cryptoRepository
                .Setup(x=>x.Exists(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            var algorithmRepository = new Mock<IAlgorithmRepository>();
            var mapper = new Mock<IMapper>();

            var handler = new AddCryptocurrencyCommandHandler(
                cryptoRepository.Object,
                algorithmRepository.Object, 
                mapper.Object);

            var result = await handler.Handle(GetCommand(), default);

            cryptoRepository
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
            var cryptoRepository = new Mock<ICryptocurrencyRepository>();
            cryptoRepository
                .Setup(x => x.Exists(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            var algorithmRepository = new Mock<IAlgorithmRepository>();
            algorithmRepository
                .Setup(x => x.Exists(It.IsAny<string>()))
                .ReturnsAsync(false);

            var mapper = new Mock<IMapper>();

            var handler = new AddCryptocurrencyCommandHandler(
                cryptoRepository.Object,
                algorithmRepository.Object,
                mapper.Object);

            var result = await handler.Handle(GetCommand(), default);

            cryptoRepository
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
