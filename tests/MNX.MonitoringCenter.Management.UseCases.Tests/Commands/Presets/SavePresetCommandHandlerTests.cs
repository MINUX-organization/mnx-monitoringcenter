using AutoMapper;
using Kernel.UseCases;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets.SavePreset;
using Moq;

namespace MNX.MonitoringCenter.Management.UseCases.Tests.Commands.Presets
{
    public class SavePresetCommandHandlerTests
    {
        [Test]
        public async Task SavePreset_ReturnsId()
        {
            Guid id = Guid.Parse("4d0b4812-6d2e-4d38-85c5-ac2c7871e000");

            var monitoringClient = new Mock<IMonitoringClient>();
            monitoringClient
                .Setup(x => x.GpuExists(It.IsAny<string>()))
                .ReturnsAsync(true);

            var presetRepository = new Mock<IPresetRepository>();
            presetRepository
                .Setup(x => x.Save(It.IsAny<Preset>()))
                .ReturnsAsync(id);

            var mapper = new Mock<IMapper>();
            mapper
                .Setup(x => x.Map<Preset>(It.IsAny<PresetModel>()))
                .Returns(new Preset());

            var handler = new SavePresetCommandHandler(
                presetRepository.Object,
                monitoringClient.Object,
                mapper.Object);

            var result = await handler.Handle(GetCommand(), default);

            presetRepository
                .Verify(x => x.Save(It.IsAny<Preset>()), Times.Once);

            Assert.NotNull(result);
            Assert.IsTrue(result.IsSuccess);
            Assert.That(result.GetValue(), Is.EqualTo(id));
        }

        [Test]
        public async Task SavePreset_WhenGpuDoesNotExist()
        {
            var monitoringClient = new Mock<IMonitoringClient>();
            monitoringClient
                .Setup(x=>x.GpuExists(It.IsAny<string>()))
                .ReturnsAsync(false);

            var presetRepository = new Mock<IPresetRepository>();
            var mapper = new Mock<IMapper>();

            var handler = new SavePresetCommandHandler(
                presetRepository.Object,
                monitoringClient.Object,
                mapper.Object);

            var result = await handler.Handle(GetCommand(), default);

            presetRepository
                .Verify(x => x.Save(It.IsAny<Preset>()), Times.Never);

            Assert.NotNull(result);
            Assert.IsFalse(result.IsSuccess);
            Assert.NotNull(result.Errors);
            Assert.That(result.Status, Is.EqualTo(ResultStatus.Invalid));
            Assert.That(result.Errors?.ElementAt(0),
                Is.EqualTo("GPU with this name wasn`t found"));
        }

        private static SavePresetCommand GetCommand()
        {
            var name = "GeForce RTX 4090";
            var memoryClock = 1313;
            var coreClock = 2235;
            var powerLimit = 450;
            var criticalTemperature = 105;
            var fanSpeed = 99;

            var presetModel = new PresetModel(
                memoryClock, coreClock, powerLimit,
                criticalTemperature, fanSpeed);

            return new SavePresetCommand(name, presetModel);
        }
    }
}
