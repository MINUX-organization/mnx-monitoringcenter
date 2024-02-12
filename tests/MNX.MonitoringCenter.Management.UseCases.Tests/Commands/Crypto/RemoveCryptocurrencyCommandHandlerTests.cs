using Kernel.UseCases;
using MediatR;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;
using MNX.MonitoringCenter.Management.UseCases.Commands.Crypto.RemoveCryptocurrency;
using Moq;
using NUnit.Framework.Constraints;
using NUnit.Framework.Internal.Commands;

namespace MNX.MonitoringCenter.Management.UseCases.Tests.Commands.Crypto
{
    public class RemoveCryptocurrencyCommandHandlerTests
    {
        [Test]
        public async Task RemoveCrypto_ReturnsEmpty()
        {
            var cryptoRepository = new Mock<ICryptocurrencyRepository>();
            var cryptocurrency = new Cryptocurrency { FullName = "Bitcoin" };
            cryptoRepository
                .Setup(x => x.GetByFullName(It.IsAny<string>()))
                .ReturnsAsync(cryptocurrency);

            var handler = new RemoveCryptocurrencyCommandHandler(
                cryptoRepository.Object);

            var result = await handler.Handle(GetCommand(), default);

            cryptoRepository
                .Verify(x => x.Remove(cryptocurrency), Times.Once());

            Assert.NotNull(result);
            Assert.IsTrue(result.IsSuccess);
            Assert.That(result, Is.EqualTo(Result<Unit>.Empty()));
        }

        [Test]
        public async Task RemoveCrypto_WhenCryptoDoesNotExist_ReturnsError()
        {
            var cryptoRepository = new Mock<ICryptocurrencyRepository>();
            cryptoRepository
                .Setup(x => x.GetByFullName(It.IsAny<string>()))
                .ReturnsAsync(null as Cryptocurrency);

            var handler = new RemoveCryptocurrencyCommandHandler(
                cryptoRepository.Object);

            var result = await handler.Handle(GetCommand(), default);

            Assert.NotNull(result);
            Assert.IsFalse(result.IsSuccess);
            Assert.NotNull(result.Errors);
            Assert.That(result.Status, Is.EqualTo(ResultStatus.Invalid));
            Assert.That(result.Errors?.ElementAt(0),
                Is.EqualTo("Cryptocurrency wasn't found"));
        }

        private static RemoveCryptocurrencyCommand GetCommand()
        {
            return new RemoveCryptocurrencyCommand("Bitcoin");
        }
    }
}
