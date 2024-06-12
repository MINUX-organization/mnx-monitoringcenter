using MediatR;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets.RemovePreset;
using Moq;

namespace MNX.MonitoringCenter.Management.UseCases.Tests.Commands.Presets.RemovePreset
{
    [TestFixture]
    public class RemovePresetCommandHandlerTests
    {
        private Mock<IPresetRepository> _presetRepository;

        private RemovePresetCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _presetRepository = new Mock<IPresetRepository>();
            _handler = new RemovePresetCommandHandler(
                _presetRepository.Object);
        }

        [Test]
        public async Task RemovePreset_ReturnsEmpty()
        {
            var preset = new Preset()
            {
                Id = Guid.NewGuid(),
                GpuName = "TestGpu",
                Name = "TestPreset",
                Overclocking = new Overclocking()
                {
                    CoreClockLock = 2000,
                    CoreClockOffset = 200,
                    MemoryClockLock = 1500,
                    MemoryClockOffset = 0,
                    CoreVoltage = 2000,
                    CoreVoltageOffset = 100,
                    MemoryVoltage = 1000,
                    MemoryVoltageOffset = 0,
                    PowerLimit = 90,
                    CriticalTemperature = 250,
                    FanSpeed = 2000
                }
            };

            _presetRepository
                .Setup(x => x.GetAvailableById(It.IsAny<Guid>(), TestHelper.Cryptocurrency.UserId))
                .ReturnsAsync(preset);

            var result = await _handler.Handle(GetCommand(), default);

            _presetRepository
                .Verify(x => x.GetAvailableById(It.IsAny<Guid>(), TestHelper.Cryptocurrency.UserId), Times.Once());
            _presetRepository
                .Verify(x => x.Remove(preset), Times.Once());

            Assert.NotNull(result);
            Assert.IsTrue(result.IsSuccess);
            Assert.That(result, Is.EqualTo(Result<Unit>.Empty()));
        }

        [Test]
        public async Task RemovePreset_WhenPresetDoesNotExist_ReturnsEmpty()
        {
            _presetRepository
                .Setup(x => x.GetAvailableById(It.IsAny<Guid>(), TestHelper.Cryptocurrency.UserId))
                .ReturnsAsync(null as Preset);

            var result = await _handler.Handle(GetCommand(), default);

            _presetRepository
                .Verify(x => x.GetAvailableById(It.IsAny<Guid>(), TestHelper.Cryptocurrency.UserId), Times.Once());
            _presetRepository
                .Verify(x => x.Remove(It.IsAny<Preset>()), Times.Never());

            Assert.NotNull(result);
            Assert.IsTrue(result.IsSuccess);
            Assert.That(result.Status, Is.EqualTo(ResultStatus.NoContent));
        }

        private static RemovePresetCommand GetCommand()
        {
            return new RemovePresetCommand(
                Guid.Parse("4d0b4812-6d2e-4d38-85c5-ac2c7871e000"), 
                TestHelper.Cryptocurrency.UserId);
        }
    }
}
