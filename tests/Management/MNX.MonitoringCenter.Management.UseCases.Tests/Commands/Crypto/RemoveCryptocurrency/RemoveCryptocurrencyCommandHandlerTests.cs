using MediatR;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Cryptocurrency;
using MNX.MonitoringCenter.Management.UseCases.Cryptocurrency.Commands.RemoveCryptocurrency;
using Moq;

namespace MNX.MonitoringCenter.Management.UseCases.Tests.Commands.Crypto.RemoveCryptocurrency
{
    [TestFixture]
    public class RemoveCryptocurrencyCommandHandlerTests
    {
        private Mock<ICryptocurrencyRepository> _cryptoRepository;

        private RemoveCryptocurrencyCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _cryptoRepository = new Mock<ICryptocurrencyRepository>();

            _handler = new RemoveCryptocurrencyCommandHandler(
                _cryptoRepository.Object);
        }

        [Test]
        public async Task RemoveCrypto_ReturnsEmpty()
        {
            var cryptocurrency = new Cryptocurrency { FullName = "Bitcoin" };
            _cryptoRepository
                .Setup(x => x.GetAvailableById(It.IsAny<Guid>(), TestHelper.Cryptocurrency.UserId))
                .ReturnsAsync(cryptocurrency);

            var result = await _handler.Handle(GetCommand(), default);

            _cryptoRepository
                .Verify(x => x.Remove(cryptocurrency), Times.Once());

            Assert.NotNull(result);
            Assert.IsTrue(result.IsSuccess);
            Assert.That(result, Is.EqualTo(Result<Unit>.Empty()));
        }

        [Test]
        public async Task RemoveCrypto_WhenCryptoDoesNotExist_ReturnsEmpty()
        {
            _cryptoRepository
                .Setup(x => x.GetAvailableById(It.IsAny<Guid>(), TestHelper.Cryptocurrency.UserId))
                .ReturnsAsync(null as Cryptocurrency);

            var result = await _handler.Handle(GetCommand(), default);

            _cryptoRepository
                .Verify(x => x.Remove(It.IsAny<Cryptocurrency>()), Times.Never());

            Assert.NotNull(result);
            Assert.IsTrue(result.IsSuccess);
            Assert.That(result.Status, Is.EqualTo(ResultStatus.NoContent));
        }

        private static RemoveCryptocurrencyCommand GetCommand()
        {
            return new RemoveCryptocurrencyCommand
                (Guid.Parse("f8b51c3b-d4eb-40b1-8465-4d16a79e429a"), 
                TestHelper.Cryptocurrency.UserId);
        }
    }
}
