using AutoMapper;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets.SavePreset;
using Moq;

namespace MNX.MonitoringCenter.Management.UseCases.Tests.Commands.Presets.SavePreset
{
    [TestFixture]
    public class SavePresetCommandHandlerTests
    {
        private Mock<IMonitoringClient> _monitoringClient;

        private Mock<IPresetRepository> _presetRepository;

        private Mock<IMapper> _mapper;

        private SavePresetCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _monitoringClient = new Mock<IMonitoringClient>();
            _presetRepository = new Mock<IPresetRepository>();
            _mapper = new Mock<IMapper>();

            _handler = new SavePresetCommandHandler(
                _presetRepository.Object,
                _monitoringClient.Object,
                _mapper.Object);
        }


        [Test]
        public async Task SavePreset_ReturnsId()
        {
            Guid id = Guid.Parse("4d0b4812-6d2e-4d38-85c5-ac2c7871e000");

            _monitoringClient
                .Setup(x => x.GpuExists(TestHelper.UserId, It.IsAny<string>()))
                .ReturnsAsync(true);

            _presetRepository
                .Setup(x => x.Save(It.IsAny<Preset>()))
                .ReturnsAsync(id);

            _mapper
                .Setup(x => x.Map<Preset>(It.IsAny<PresetInputModel>()))
                .Returns(new Preset());

            var result = await _handler.Handle(GetCommand(), default);

            _presetRepository
                .Verify(x => x.Save(It.IsAny<Preset>()), Times.Once);

            Assert.NotNull(result);
            Assert.IsTrue(result.IsSuccess);
            Assert.That(result.GetValue(), Is.EqualTo(id));
        }

        [Test]
        public async Task SavePreset_WhenGpuDoesNotExist()
        {
            _monitoringClient
                .Setup(x => x.GpuExists(TestHelper.UserId, It.IsAny<string>()))
                .ReturnsAsync(false);

            var result = await _handler.Handle(GetCommand(), default);

            _presetRepository
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
            var gpuName = "GeForce RTX 4090";
            var memoryClock = 1313;
            var coreClock = 2235;
            var powerLimit = 450;
            var criticalTemperature = 105;
            var fanSpeed = 99;
            
            var presetModel = new PresetInputModel(
                memoryClock, coreClock, powerLimit,
                criticalTemperature, fanSpeed);

            var savePresetInputModel = new SavePresetInputModel(gpuName, presetModel);

            return new SavePresetCommand(TestHelper.UserId, savePresetInputModel);
        }
    }
}
