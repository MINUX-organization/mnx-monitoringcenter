using FluentValidation;

namespace MINUX.Backend.Worker.UseCases.Commands.Presets.SavePreset;

public class SavePresetValidator : AbstractValidator<SavePresetCommand>
{
    public SavePresetValidator()
    {
        RuleFor(x => x.GpuName)
            .NotEmpty()
            .WithMessage("Название GPU не должно быть пустым");

        RuleFor(x => x.Model)
            .NotNull()
            .WithMessage("Данные для пресета обязательны")
            .DependentRules(() =>
            {
                RuleFor(x => x.Model.CoreClock)
                    .Must(coreClock => 1000 <= coreClock && coreClock <= 5000)
                    .WithMessage("Значение тактовой чатсоты ядра не должно выходить за диапазон [1000; 5000] Мгц");

                RuleFor(x => x.Model.MemoryClock)
                    .Must(memoryClock => 1000 <= memoryClock && memoryClock <= 5000)
                    .WithMessage("Значение тактовой чатсоты памяти не должно выходить за диапазон [1000; 5000] Мгц");

                RuleFor(x => x.Model.PowerLimit)
                    .Must(powerLimit => 100 <= powerLimit && powerLimit <= 150)
                    .WithMessage("Значение ограничения мощности не должно выходить за диапазон [100; 150] Ватт");

                RuleFor(x => x.Model.CriticalTemperature)
                    .Must(criticalTemperature => 100 <= criticalTemperature && criticalTemperature <= 150)
                    .WithMessage("Значение критической температуры не должно выходить за диапазон [0; 110] гадусов Цельсия");

                RuleFor(x => x.Model.FanSpeed)
                    .Must(fanSpeed => 0 <= fanSpeed && fanSpeed <= 100)
                    .WithMessage("Значение скорости вентилятора не должно выходить за диапазон [0; 100] %");
            });
    }
}
