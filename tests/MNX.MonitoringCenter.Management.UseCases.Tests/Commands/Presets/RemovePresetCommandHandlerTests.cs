using Kernel.UseCases;
using MediatR;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets.RemovePreset;
using Moq;

namespace MNX.MonitoringCenter.Management.UseCases.Tests.Commands.Presets
{
    public class RemovePresetCommandHandlerTests
    {
        [Test]
        public async Task RemovePreset_ReturnsEmpty()
        {
            var presetRepository = new Mock<IPresetRepository>();

            var preset = new Preset
            {
                Id = Guid.Parse("4d0b4812-6d2e-4d38-85c5-ac2c7871e000"),
                GpuName = "GeForce RTX 4090",
                MemoryClock = 1313,
                CoreClock = 2235,
                PowerLimit = 450,
                CriticalTemperature = 105,
                FanSpeed = 99,
            };

            presetRepository
                .Setup(x => x.GetById(It.IsAny<Guid>()))
                .ReturnsAsync(preset);

            var handler = new RemovePresetCommandHandler(
                presetRepository.Object);

            var result = await handler.Handle(GetCommand(), default);

            presetRepository
                .Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once());
            presetRepository
                .Verify(x => x.Remove(preset), Times.Once());

            Assert.NotNull(result);
            Assert.IsTrue(result.IsSuccess);
            Assert.That(result, Is.EqualTo(Result<Unit>.Empty()));
        }

        [Test]
        public async Task RemovePreset_WhenPresetDoesNotExist_ReturnsError()
        {
            var presetRepository = new Mock<IPresetRepository>();
            presetRepository
                .Setup(x => x.GetById(It.IsAny<Guid>()))
                .ReturnsAsync(null as Preset);

            var handler = new RemovePresetCommandHandler(
                presetRepository.Object);

            var result = await handler.Handle(GetCommand(), default);

            presetRepository
                .Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once());

            Assert.NotNull(result);
            Assert.IsFalse(result.IsSuccess);
            Assert.NotNull(result.Errors);
            Assert.That(result.Status, Is.EqualTo(ResultStatus.Invalid));
            Assert.That(result.Errors?.ElementAt(0),
                Is.EqualTo("Preset with this id wasn`t found"));
        }

        private static RemovePresetCommand GetCommand()
        {
            return new RemovePresetCommand(
                Guid.Parse("4d0b4812-6d2e-4d38-85c5-ac2c7871e000"));
        }
    }
}
