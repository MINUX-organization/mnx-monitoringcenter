using MNX.MonitoringCenter.Management.UseCases.Commands.Presets;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets.UpdatePreset;
using FluentValidation.TestHelper;

namespace MNX.MonitoringCenter.Management.UseCases.Tests.Commands.Presets.UpdatePreset
{
    [TestFixture]
    public class UpdatePresetValidatorTests
    {
        private UpdatePresetValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new UpdatePresetValidator();
        }

        [Test]
        public void SavePresetCommand_WhenModelNotNull_ShouldNotErrors()
        {
            var command = GetCommand(CreatePresetModel());
            var result = _validator.TestValidate(command);

            result.ShouldNotHaveValidationErrorFor(x => x.Model);
        }

        [Test]
        public void SavePresetCommand_WhenModelNull_ShouldErrors()
        {
            var command = GetCommand(null!);
            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Model)
                  .WithErrorMessage("Данные для пресета обязательны");
        }

        [TestCaseSource(typeof(PresetCommandTestCase),
                        nameof(PresetCommandTestCase.CreateCorrectPresetModel))]
        public void UpdatePresetCommand_WhenPresetModelAreValid_ShouldNotErrors(PresetModel model)
        {
            var command = new UpdatePresetCommand(Guid.NewGuid(), model);
            var result = _validator.TestValidate(command);

            result.ShouldNotHaveValidationErrorFor(x => x.Model.CoreClock);

            result.ShouldNotHaveValidationErrorFor(x => x.Model.MemoryClock);

            result.ShouldNotHaveValidationErrorFor(x => x.Model.PowerLimit);

            result.ShouldNotHaveValidationErrorFor(x => x.Model.CriticalTemperature);

            result.ShouldNotHaveValidationErrorFor(x => x.Model.FanSpeed);
        }

        [TestCaseSource(typeof(PresetCommandTestCase),
                        nameof(PresetCommandTestCase.CreateIncorrectPresetModel))]
        public void UpdatePresetCommand_WhenPresetModelAreNotValid_ShouldErrors(PresetModel model)
        {
            var command = new UpdatePresetCommand(Guid.NewGuid(), model);
            var result = _validator.TestValidate(command);

            result.ShouldNotHaveValidationErrorFor(x => x.Model);

            result.ShouldHaveValidationErrorFor(x => x.Model.CoreClock)
                  .WithErrorMessage("Значение тактовой частоты ядра" +
                        " не должно выходить за диапазон [1000; 5000] Мгц");

            result.ShouldHaveValidationErrorFor(x => x.Model.MemoryClock)
                  .WithErrorMessage("Значение тактовой частоты памяти" +
                        " не должно выходить за диапазон [1000; 5000] Мгц");

            result.ShouldHaveValidationErrorFor(x => x.Model.PowerLimit)
                  .WithErrorMessage("Значение ограничения мощности не" +
                        " должно выходить за диапазон [100; 150] Ватт");

            result.ShouldHaveValidationErrorFor(x => x.Model.CriticalTemperature)
                  .WithErrorMessage("Значение критической температуры" +
                        " не должно выходить за диапазон [0; 110] гадусов" +
                        " Цельсия");

            result.ShouldHaveValidationErrorFor(x => x.Model.FanSpeed)
                  .WithErrorMessage("Значение скорости вентилятора не" +
                        " должно выходить за диапазон [0; 100] %");
        }

        private static UpdatePresetCommand GetCommand(PresetModel presetModel)
        {
            Guid id = Guid.Parse("4d0b4812-6d2e-4d38-85c5-ac2c7871e000");

            return new UpdatePresetCommand(id, presetModel);
        }

        private static PresetModel CreatePresetModel()
        {
            return new PresetModel(memoryClock: 1313,
                                   coreClock: 2235,
                                   powerLimit: 150,
                                   criticalTemperature: 105,
                                   fanSpeed: 99);
        }
    }
}
