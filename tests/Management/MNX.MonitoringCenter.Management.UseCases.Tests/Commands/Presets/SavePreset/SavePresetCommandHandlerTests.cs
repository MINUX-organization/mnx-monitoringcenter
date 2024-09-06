using AutoMapper;
using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets.SavePreset;
using MNX.MonitoringCenter.Management.UseCases.Presets;
using Moq;

namespace MNX.MonitoringCenter.Management.UseCases.Tests.Commands.Presets.SavePreset
{
    [TestFixture]
    public class SavePresetCommandHandlerTests
    {
        private Mock<IMonitoringClient> _monitoringClient;

        private Mock<IPresetRepository> _presetRepository;

        private IMapper _mapper = TestHelper.GetMapper();

        private SavePresetCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _monitoringClient = new Mock<IMonitoringClient>();
            _presetRepository = new Mock<IPresetRepository>();

            _handler = new SavePresetCommandHandler(
                _presetRepository.Object,
                _monitoringClient.Object,
                _mapper);
        }


        [Test]
        public async Task SavePreset_ReturnsPresetModel()
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
                .Setup(x => x.Exists(TestHelper.UserId, It.IsAny<string>()))
                .ReturnsAsync(false);

            var result = await _handler.Handle(GetCommand(), default);

            _presetRepository
                .Verify(x => x.Save(It.IsAny<Preset>()), Times.Once);

            Assert.NotNull(result);
            Assert.IsTrue(result.IsSuccess);           
            Assert.IsTrue(result.GetValue().Name == GetCommand().SavePresetModel.Name &&
                          result.GetValue().GpuName == GetCommand().SavePresetModel.GpuName &&
                          OverclockingEquals(result.GetValue().Overclocking, GetCommand().SavePresetModel.Overclocking),
                          "The return value does not match the expected value");
        }

        //[Test]
        //public async Task SavePreset_WhenGpuDoesNotExist()
        //{
        //    _monitoringClient
        //        .Setup(x => x.GpuExists(TestHelper.UserId, It.IsAny<string>()))
        //        .ReturnsAsync(false);

        //    var result = await _handler.Handle(GetCommand(), default);

        //    _presetRepository
        //        .Verify(x => x.Save(It.IsAny<Preset>()), Times.Never);

        //    Assert.NotNull(result);
        //    Assert.IsFalse(result.IsSuccess);
        //    Assert.NotNull(result.Errors);
        //    Assert.That(result.Status, Is.EqualTo(ResultStatus.Invalid));
        //    Assert.That(result.Errors?.ElementAt(0),
        //        Is.EqualTo("GPU with this name wasn`t found"));
        //}

        private static SavePresetCommand GetCommand()
        {
            return new SavePresetCommand(TestHelper.UserId, 
                new SavePresetInputModel("TestPreset", "TestGpu", 
                new OverclockingInputModel(2000, 200, 1500, 0, 2000, 100, 1000, 0, 90, 250, 2000)));        
        }

        private bool OverclockingEquals(OverclockingModel result, OverclockingInputModel expected)
        {
            var overclocking = _mapper.Map<OverclockingInputModel>(result);

            if (result.CoreClockLock == expected.CoreClockLock &&
                result.CoreClockOffset == expected.CoreClockOffset &&
                result.CoreVoltage == expected.CoreVoltage &&
                result.CoreVoltageOffset == expected.CoreVoltageOffset &&
                result.MemoryClockLock == expected.MemoryClockLock &&
                result.MemoryClockOffset == expected.MemoryClockOffset &&
                result.MemoryVoltage == expected.MemoryVoltage &&
                result.MemoryVoltageOffset == expected.MemoryVoltageOffset &&
                result.CriticalTemperature == expected.CriticalTemperature &&
                result.PowerLimit == expected.PowerLimit &&
                result.FanSpeed == expected.FanSpeed) return true;
             
            else return false;
        }
    }
}
