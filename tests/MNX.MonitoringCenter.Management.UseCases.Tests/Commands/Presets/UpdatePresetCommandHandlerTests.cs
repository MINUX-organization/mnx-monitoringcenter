using AutoMapper;
using Kernel.UseCases;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets.SavePreset;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets;
using Moq;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets.RemovePreset;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets.UpdatePreset;
using MediatR;

namespace MNX.MonitoringCenter.Management.UseCases.Tests.Commands.Presets
{
    public class UpdatePresetCommandHandlerTests
    {
        [Test]
        public async Task UpdatePreset_ReturnsEmpty()
        {
            var presetRepository = new Mock<IPresetRepository>();
            presetRepository
                .Setup(x => x.GetById(It.IsAny<Guid>()))
                .ReturnsAsync(new Preset());

            presetRepository
                .Setup(x => x.Update(It.IsAny<Preset>()));

            var mapper = new Mock<IMapper>();
            mapper
                .Setup(x => x.Map<Preset>(It.IsAny<PresetModel>()))
                .Returns(new Preset());

            var handler = new UpdatePresetCommandHandler(
                presetRepository.Object,
                mapper.Object);

            var result = await handler.Handle(GetCommand(), default);

            presetRepository
                .Verify(x => x.Update(It.IsAny<Preset>()), Times.Once);

            Assert.NotNull(result);
            Assert.IsTrue(result.IsSuccess);
            Assert.That(result, Is.EqualTo(Result<Unit>.Empty()));
        }

        [Test]
        public async Task UpdatePreset_WhenPresetDoesNotExist_ReturnsError()
        {
            var presetRepository = new Mock<IPresetRepository>();
            presetRepository
                .Setup(x => x.GetById(It.IsAny<Guid>()))
                .ReturnsAsync(null as Preset);

            var mapper = new Mock<IMapper>();

            var handler = new UpdatePresetCommandHandler(
                presetRepository.Object,
                mapper.Object);

            var result = await handler.Handle(GetCommand(), default);

            presetRepository
                .Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once());

            Assert.NotNull(result);
            Assert.IsFalse(result.IsSuccess);
            Assert.NotNull(result.Errors);
            Assert.That(result.Status, Is.EqualTo(ResultStatus.Invalid));
            Assert.That(result.Errors?.ElementAt(0),
                Is.EqualTo("Preset with this Id wasn`t found"));
        }

        private static UpdatePresetCommand GetCommand()
        {
            var memoryClock = 1313;
            var coreClock = 2235;
            var powerLimit = 450;
            var criticalTemperature = 105;
            var fanSpeed = 99;

            var presetModel = new PresetModel(
                memoryClock, coreClock, powerLimit,
                criticalTemperature, fanSpeed);

            return new UpdatePresetCommand(
                Guid.Parse("4d0b4812-6d2e-4d38-85c5-ac2c7871e000"),
                presetModel);
        }
    }
}
