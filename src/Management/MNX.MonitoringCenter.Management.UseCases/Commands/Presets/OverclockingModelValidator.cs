using FluentValidation;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Presets;

/// <summary>
/// Валидатор модели разгона
/// </summary>
public class OverclockingModelValidator : AbstractValidator<OverclockingInputModel>
{
    public OverclockingModelValidator()
    {
        RuleFor(x => x.CoreClockLock)
            .Must(coreClockLock => 1000 <= coreClockLock && coreClockLock <= 3000)
            .WithMessage("The value of the core clock lock must not exceed the range [1000; 3000]");

        RuleFor(x => x.CoreClockOffset)
            .Must(coreClockOffset => 100 <= coreClockOffset && coreClockOffset <= 300)
            .WithMessage("The value of the core clock offset must not exceed the range [100; 300]");

        RuleFor(x => x.CoreVoltage)
            .Must(coreVoltage => 1000 <= coreVoltage && coreVoltage <= 2000)
            .WithMessage("The value of the core voltage must not exceed the range [1000; 2000]");

        RuleFor(x => x.CoreVoltageOffset)
            .Must(coreVoltageOffset => -50 <= coreVoltageOffset && coreVoltageOffset <= 50)
            .WithMessage("The value of the core voltage offset must not exceed the range [-50; 50]");

        RuleFor(x => x.MemoryClockLock)
            .Must(memoryClockLock => 1000 <= memoryClockLock && memoryClockLock <= 3000)
            .WithMessage("The value of the memory clock lock must not exceed the range [1000; 3000]");

        RuleFor(x => x.MemoryClockOffset)
            .Must(memoryClockOffset => 50 <= memoryClockOffset && memoryClockOffset <= 150)
            .WithMessage("The value of the memory clock offset must not exceed the range [50; 150]");

        RuleFor(x => x.MemoryVoltage)
            .Must(memoryVoltage => 900 <= memoryVoltage && memoryVoltage <= 1100)
            .WithMessage("The value of the memory voltage must not exceed the range [900; 1100]");

        RuleFor(x => x.MemoryVoltageOffset)
            .Must(memoryVoltageOffset => -50 <= memoryVoltageOffset && memoryVoltageOffset <= 50)
            .WithMessage("The value of the memory voltage offset must not exceed the range [-50; 50]");

        RuleFor(x => x.CriticalTemperature)
            .Must(criticalTemperature => 80 <= criticalTemperature && criticalTemperature <= 100)
            .WithMessage("The value of the critical temperature must not exceed the range [80; 100]");

        RuleFor(x => x.PowerLimit)
            .Must(powerLimit => 100 <= powerLimit && powerLimit <= 350)
            .WithMessage("The value of the power limit must not exceed the range [100; 350]");

        RuleFor(x => x.FanSpeed)
            .Must(fanSpeed => 500 <= fanSpeed && fanSpeed <= 3000)
            .WithMessage("The value of the fan speed must not exceed the range [500; 3000]");
    }
}
