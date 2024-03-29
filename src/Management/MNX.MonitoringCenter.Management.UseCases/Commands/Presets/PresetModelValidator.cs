using FluentValidation;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Presets;

/// <summary>
/// Валидатор модели пресета
/// </summary>
public class PresetModelValidator : AbstractValidator<PresetInputModel>
{
    public PresetModelValidator()
    {
        RuleFor(x => x.CoreClock)
            .Must(coreClock => 1000 <= coreClock && coreClock <= 5000)
            .WithMessage("The value of the core clock frequency must not exceed the range [1000; 5000] MHz");

        RuleFor(x => x.MemoryClock)
            .Must(memoryClock => 1000 <= memoryClock && memoryClock <= 5000)
            .WithMessage("The memory clock frequency value must not exceed the range [1000; 5000] MHz");

        RuleFor(x => x.PowerLimit)
            .Must(powerLimit => 100 <= powerLimit && powerLimit <= 150)
            .WithMessage("The power limitation value must not exceed the range [100; 150] Watts");

        RuleFor(x => x.CriticalTemperature)
            .Must(criticalTemperature => 0 <= criticalTemperature && criticalTemperature <= 110)
            .WithMessage("The value of the critical temperature must not exceed the range [0; 110] gradus Celsius");

        RuleFor(x => x.FanSpeed)
            .Must(fanSpeed => 0 <= fanSpeed && fanSpeed <= 100)
            .WithMessage("The fan speed value must not exceed the range [0; 100] %");
    }
}
