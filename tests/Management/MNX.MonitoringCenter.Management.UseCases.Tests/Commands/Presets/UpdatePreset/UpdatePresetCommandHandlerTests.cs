using AutoMapper;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets.SavePreset;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets.UpdatePreset;
using MNX.MonitoringCenter.Management.UseCases.Presets;
using Moq;

namespace MNX.MonitoringCenter.Management.UseCases.Tests.Commands.Presets.UpdatePreset
{
    [TestFixture]
    public class UpdatePresetCommandHandlerTests
    {
        private Mock<IPresetRepository> _presetRepository;

        private IMapper _mapper = TestHelper.GetMapper(); 

        private UpdatePresetCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _presetRepository = new Mock<IPresetRepository>();

            _handler = new UpdatePresetCommandHandler(
                _presetRepository.Object,
                _mapper);
        }

        [Test]
        public async Task UpdatePreset_ReturnsEmpty()
        {
            _presetRepository
                .Setup(x => x.GetAvailableById(It.IsAny<Guid>(), TestHelper.UserId))
                .ReturnsAsync(new Preset());

            _presetRepository
                .Setup(x => x.Update(It.IsAny<Preset>()));

            var result = await _handler.Handle(GetCommand(), default);

            _presetRepository
                .Verify(x => x.Update(It.IsAny<Preset>()), Times.Once);

            Assert.NotNull(result);
            Assert.IsTrue(result.IsSuccess);
            Assert.That(result, Is.EqualTo(Result<PresetModel>.Empty()));
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
            var presetModel = new SavePresetInputModel("Test", "Test",
                new OverclockingInputModel(2000, 200, 1500, 0, 2000, 100, 1000, 0, 90, 250, 2000));

            return new UpdatePresetCommand(
                Guid.Parse("4d0b4812-6d2e-4d38-85c5-ac2c7871e000"),
                presetModel, 
                TestHelper.UserId);
        }
    }
}
