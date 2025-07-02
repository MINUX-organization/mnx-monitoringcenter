using FluentValidation;
using MNX.MonitoringCenter.Inventory.Contracts.Devices;

namespace MNX.MonitoringCenter.Management.UseCases.SetRigDevices.DevicesValidation.GpuModelValidation.Restrictions;

/// <summary>
/// Валидатор параметров сущности <see cref="IntegerTypeRestrictions"/>.
/// </summary>
public class GpuIntegerTypeRestrictionsValidator : AbstractValidator<IntegerTypeRestrictions>
{
    ///
    public GpuIntegerTypeRestrictionsValidator() { }

    ///
    public GpuIntegerTypeRestrictionsValidator(string propertyName)
    {
        RuleFor(x => x.Minimal)
            .LessThanOrEqualTo(x => x.Maximal)
            .WithMessage($"{propertyName}.Minimal must be equal or less than {propertyName}.Maximal");

        RuleFor(x => x.Maximal)
            .GreaterThanOrEqualTo(x => x.Minimal)
            .WithMessage($"{propertyName}.Maximal must be equal or greater than {propertyName}.Minimal");
    }
}
