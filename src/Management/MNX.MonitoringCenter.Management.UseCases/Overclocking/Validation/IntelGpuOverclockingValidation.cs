using FluentValidation;
using MNX.MonitoringCenter.Management.Core.Overclocking.Gpu;

namespace MNX.MonitoringCenter.Management.UseCases.Overclocking.Validation;

/// <summary>
/// Валидатор разгона видеокарты параметра Intel.
/// </summary>
public class IntelGpuOverclockingValidation : OverclockingValidatorBase<IntelGpuOverclocking>
{
    ///
    public IntelGpuOverclockingValidation()
    {
        RuleFor(model => model)
            .Custom((model, context) =>
            {
                context.AddFailure($"{nameof(model)} does not supporting at this moment");
            });
    }
}
