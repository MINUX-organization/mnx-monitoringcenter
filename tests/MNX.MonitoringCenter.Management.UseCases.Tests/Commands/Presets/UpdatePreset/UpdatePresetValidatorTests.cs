using MNX.MonitoringCenter.Management.UseCases.Commands.Presets;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets.UpdatePreset;
using FluentValidation.TestHelper;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets.SavePreset;

namespace MNX.MonitoringCenter.Management.UseCases.Tests.Commands.Presets.UpdatePreset
{
    [TestFixture]
    public class UpdatePresetValidatorTests
    {
        private UpdatePresetValidator _validator;

        private UpdatePresetCommand? _command;

        private TestValidationResult<UpdatePresetCommand>? _result;

        [SetUp]
        public void Setup()
        {
            _validator = new UpdatePresetValidator();
        }

        [Test]
        public void ModelNotNull()
        {
            _command = GetCommand(CreatePresetModel());
            _result = _validator.TestValidate(_command);

            _result.ShouldNotHaveValidationErrorFor(x => x.Model);
        }

        [Test]
        public void ModelNull()
        {
            _command = GetCommand(null);
            _result = _validator.TestValidate(_command);

            _result.ShouldHaveValidationErrorFor(x => x.Model)
                .WithErrorMessage("Данные для пресета обязательны");
        }

        [Test]
        [TestCaseSource(typeof(PresetCommandTestCase),
                    nameof(PresetCommandTestCase.GetCommandTestCases))]
        public void PropertiesModelAreValid(PresetModel presetModel)
        {
            _command = GetCommand(presetModel);
            _result = _validator.TestValidate(_command);

            _result.ShouldNotHaveValidationErrorFor(x => x.Model);

            var errorsList = _result.Errors.ToList();

            if (errorsList.Count != 0)
            {
                var errorsMessageList = new List<string>();

                foreach (var error in errorsList)
                {
                    errorsMessageList.Add(error.ErrorMessage);
                }

                if (errorsMessageList.Contains("Значение тактовой частоты" +
                    " ядра не должно выходить за диапазон [1000; 5000] Мгц"))
                {
                    _result
                        .ShouldHaveValidationErrorFor(x => x.Model.CoreClock)
                        .WithErrorMessage("Значение тактовой частоты ядра" +
                        " не должно выходить за диапазон [1000; 5000] Мгц");
                }

                if (errorsMessageList.Contains("Значение тактовой частоты" +
                    " памяти не должно выходить за диапазон [1000; 5000] Мгц"))
                {
                    _result
                        .ShouldHaveValidationErrorFor(x => x.Model.MemoryClock)
                        .WithErrorMessage("Значение тактовой частоты памяти" +
                        " не должно выходить за диапазон [1000; 5000] Мгц");
                }

                if (errorsMessageList.Contains("Значение ограничения " +
                    "мощности не должно выходить за диапазон [100; 150] Ватт"))
                {
                    _result
                        .ShouldHaveValidationErrorFor(x => x.Model.PowerLimit)
                        .WithErrorMessage("Значение ограничения мощности не" +
                        " должно выходить за диапазон [100; 150] Ватт");
                }

                if (errorsMessageList.Contains("Значение критической" +
                    " температуры не должно выходить за диапазон [0; 110]" +
                    " гадусов Цельсия"))
                {
                    _result
                        .ShouldHaveValidationErrorFor(
                            x => x.Model.CriticalTemperature)
                        .WithErrorMessage("Значение критической температуры" +
                        " не должно выходить за диапазон [0; 110] гадусов" +
                        " Цельсия");
                }

                if (errorsMessageList.Contains("Значение скорости" +
                    " вентилятора не должно выходить за диапазон [0; 100] %"))
                {
                    _result
                        .ShouldHaveValidationErrorFor(
                            x => x.Model.FanSpeed)
                        .WithErrorMessage("Значение скорости вентилятора не" +
                        " должно выходить за диапазон [0; 100] %");
                }
            }
            else
            {
                _result.ShouldNotHaveValidationErrorFor(
                x => x.Model.CoreClock);
                _result.ShouldNotHaveValidationErrorFor(
                    x => x.Model.MemoryClock);
                _result.ShouldNotHaveValidationErrorFor(
                    x => x.Model.PowerLimit);
                _result.ShouldNotHaveValidationErrorFor(
                    x => x.Model.CriticalTemperature);
                _result.ShouldNotHaveValidationErrorFor(
                    x => x.Model.FanSpeed);
            }
        }

        private static UpdatePresetCommand GetCommand(PresetModel presetModel)
        {
            Guid id = Guid.Parse("4d0b4812-6d2e-4d38-85c5-ac2c7871e000");

            return new UpdatePresetCommand(id, presetModel);
        }

        private static PresetModel CreatePresetModel()
        {
            var memoryClock = 1313;
            var coreClock = 5235;
            var powerLimit = 150;
            var criticalTemperature = 105;
            var fanSpeed = 99;

            return new PresetModel(
                memoryClock, coreClock, powerLimit,
                criticalTemperature, fanSpeed);
        }
    }
}
