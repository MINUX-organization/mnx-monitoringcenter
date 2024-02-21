using MNX.MonitoringCenter.Management.UseCases.Commands.Presets.SavePreset;
using FluentValidation.TestHelper;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets;

namespace MNX.MonitoringCenter.Management.UseCases.Tests.Commands.Presets.SavePreset
{
    [TestFixture]
    public class SavePresetValidatorTests
    {
        private SavePresetValidator _validator;

        private SavePresetCommand? _command;

        private TestValidationResult<SavePresetCommand>? _result;

        [SetUp]
        public void Setup()
        {
            _validator = new SavePresetValidator();
        }

        [Test]
        public void GpuNameNotEmpty()
        {
            _command = GetCommand("GeForce RTX 4090");
            _result = _validator.TestValidate(_command);

            _result.ShouldNotHaveValidationErrorFor(x => x.GpuName);
        }

        [Test]
        public void GpuNameEmpty()
        {
            _command = GetCommand("");
            _result = _validator.TestValidate(_command);

            _result.ShouldHaveValidationErrorFor(x => x.GpuName)
                .WithErrorMessage("Название GPU не должно быть пустым");
        }

        [Test]
        [TestCaseSource(typeof(PresetCommandTestCase),
                    nameof(PresetCommandTestCase.GetCommandTestCases))]
        public void PropertiesModelAreValid(PresetModel presetModel)
        {
            _command = new SavePresetCommand("GeForce RTX 4090", presetModel);
            _result = _validator.TestValidate(_command);

            _result.ShouldNotHaveValidationErrorFor(
                x => x.Model);

            var errorsList = _result.Errors.ToList();

            if (errorsList.Count != 0)
            {
                var errorsMessageList =
                    errorsList.Select(error => error.ErrorMessage).ToList();

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

        [Test]
        public void ModelNotNull()
        {
            _command = GetCommand("GeForce RTX 4090");
            _result = _validator.TestValidate(_command);

            _result.ShouldNotHaveValidationErrorFor(
                x => x.Model);
        }

        [Test]
        public void ModelNull()
        {
            _command = new SavePresetCommand("GeForce RTX 4090", null);
            _result = _validator.TestValidate(_command);

            _result.ShouldHaveValidationErrorFor(x => x.Model)
                .WithErrorMessage("Данные для пресета обязательны");
        }

        private static SavePresetCommand GetCommand(string name)
        {
            var presetModel = CreatePresetModel();

            return new SavePresetCommand(name, presetModel);
        }

        private static PresetModel CreatePresetModel()
        {
            var memoryClock = 1313;
            var coreClock = 2235;
            var powerLimit = 150;
            var criticalTemperature = 105;
            var fanSpeed = 99;

            return new PresetModel(
                memoryClock, coreClock, powerLimit,
                criticalTemperature, fanSpeed);
        }

        private static PresetModel CreateIncorrectPresetModel()
        {
            var memoryClock = 1;
            var coreClock = 1;
            var powerLimit = 1;
            var criticalTemperature = 1;
            var fanSpeed = 1;

            return new PresetModel(
                memoryClock, coreClock, powerLimit,
                criticalTemperature, fanSpeed);
        }
    }
}
