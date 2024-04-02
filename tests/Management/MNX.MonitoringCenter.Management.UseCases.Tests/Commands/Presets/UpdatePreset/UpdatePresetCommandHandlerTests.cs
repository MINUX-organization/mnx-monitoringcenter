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

namespace MNX.MonitoringCenter.Management.UseCases.Tests.Commands.Presets.UpdatePreset
{
    [TestFixture]
    public class UpdatePresetCommandHandlerTests
    {
        private Mock<IPresetRepository> _presetRepository;

        private Mock<IMapper> _mapper;

        private UpdatePresetCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _presetRepository = new Mock<IPresetRepository>();
            _mapper = new Mock<IMapper>();

            _handler = new UpdatePresetCommandHandler(
                _presetRepository.Object,
                _mapper.Object);
        }

        [Test]
        public async Task UpdatePreset_ReturnsEmpty()
        {
            _presetRepository
                .Setup(x => x.GetAvailableById(It.IsAny<Guid>(), TestHelper.UserId))
                .ReturnsAsync(new Preset());

            _presetRepository
                .Setup(x => x.Update(It.IsAny<Preset>()));

            _mapper
                .Setup(x => x.Map<Preset>(It.IsAny<PresetInputModel>()))
                .Returns(new Preset());

            var result = await _handler.Handle(GetCommand(), default);

            _presetRepository
                .Verify(x => x.Update(It.IsAny<Preset>()), Times.Once);

            Assert.NotNull(result);
            Assert.IsTrue(result.IsSuccess);
            Assert.That(result, Is.EqualTo(Result<Unit>.Empty()));
        }

        [Test]
        public async Task UpdatePreset_WhenPresetDoesNotExist_ReturnsError()
        {
            _presetRepository
                .Setup(x => x.GetAvailableById(It.IsAny<Guid>(), TestHelper.UserId))
                .ReturnsAsync(null as Preset);

            var result = await _handler.Handle(GetCommand(), default);

            _presetRepository
                .Verify(x => x.GetAvailableById(It.IsAny<Guid>(), TestHelper.UserId), Times.Once());

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

            var presetModel = new PresetInputModel(
                memoryClock, coreClock, powerLimit,
                criticalTemperature, fanSpeed);

            return new UpdatePresetCommand(
                Guid.Parse("4d0b4812-6d2e-4d38-85c5-ac2c7871e000"),
                presetModel, 
                TestHelper.UserId);
        }
    }
}
