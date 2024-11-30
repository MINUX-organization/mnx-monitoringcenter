using FluentValidation;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Restrictions;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets;

namespace MNX.MonitoringCenter.Management.UseCases.Presets.Commands;

/// <summary>
/// Валидатор входной модели разгона.
/// </summary>
public class OverclockingInputModelValidator : AbstractValidator<OverclockingInputModel>
{
    public OverclockingInputModelValidator() { } // нужен для инициализации пайплайна валидации.

    public OverclockingInputModelValidator(GpuRestrictions restrictions)
    {
        RuleFor(x => x)
            .NotNull()
            .WithMessage("Restrictions is required");

        RuleFor(x => x.PowerLimit)
            .NotNull()
            .WithMessage("Power limit must not be null")
            .InclusiveBetween(restrictions!.Power.Minimal, restrictions.Power.Maximal);

        RuleFor(x => x.FanSpeed)
            .NotNull()
            .WithMessage("Fan speed must not be null")
            .InclusiveBetween(restrictions.FanSpeed.Minimal, restrictions.FanSpeed.Maximal);

        RuleFor(x => x.CoreVoltage)
            .NotNull()
            .WithMessage("Core voltage must not be null")
            .InclusiveBetween(restrictions.Voltage.Core.Lock.Minimal, restrictions.Voltage.Core.Lock.Maximal);

        RuleFor(x => x.CoreVoltageOffset)
            .NotNull()
            .WithMessage("Core voltage offset must not be null")
            .InclusiveBetween(restrictions.Voltage.Core.Offset.Minimal, restrictions.Voltage.Core.Offset.Maximal);

        RuleFor(x => x.MemoryVoltage)
            .NotNull()
            .WithMessage("Memory voltage must not be null")
            .InclusiveBetween(restrictions.Voltage.Memory.Lock.Minimal, restrictions.Voltage.Memory.Lock.Maximal);

        RuleFor(x => x.MemoryVoltageOffset)
            .NotNull()
            .WithMessage("Memory voltage offset must not be null")
            .InclusiveBetween(restrictions.Voltage.Memory.Offset.Minimal, restrictions.Voltage.Memory.Offset.Maximal);

        RuleFor(x => x.CoreClockLock)
            .NotNull()
            .WithMessage("Core clock lock must not be null")
            .InclusiveBetween(restrictions.Clock.Core.Lock.Minimal, restrictions.Clock.Core.Lock.Maximal);

        RuleFor(x => x.CoreClockOffset)
            .NotNull()
            .WithMessage("Core clock offset must not be null")
            .InclusiveBetween(restrictions.Clock.Core.Offset.Minimal, restrictions.Clock.Core.Offset.Maximal);

        RuleFor(x => x.MemoryClockLock)
            .NotNull()
            .WithMessage("Memory clock lock must not be null")
            .InclusiveBetween(restrictions.Clock.Memory.Lock.Minimal, restrictions.Clock.Memory.Lock.Maximal);

        RuleFor(x => x.MemoryClockOffset)
            .NotNull()
            .WithMessage("Memory clock offset must not be null")
            .InclusiveBetween(restrictions.Clock.Memory.Offset.Minimal, restrictions.Clock.Memory.Offset.Maximal);
    }
}
