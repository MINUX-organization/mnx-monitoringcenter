using FluentValidation;
using MNX.MonitoringCenter.Inventory.Contracts.Devices;
using MNX.MonitoringCenter.Inventory.UseCases.RigInventory.RigInventoryValidation.GpuModelValidation.Restrictions;
using System.Linq.Expressions;

namespace MNX.MonitoringCenter.Inventory.UseCases.RigInventory.RigInventoryValidation.GpuModelValidation;

/// <summary>
/// Статический класс расширение методов валидации данных инвентаризации.
/// </summary>
public static class GpuValidationExtensions
{
    /// <summary>
    /// Валидировать параметры класса <see cref="IntegerTypeRestrictions"/>.
    /// </summary>
    /// <param name="validator"> Валидатор. </param>
    /// <param name="selector"> Валидируемый параметр. </param>
    public static void ValidateRangeValues<TModel>(
        this AbstractValidator<TModel> validator,
        Expression<Func<TModel, IntegerTypeRestrictions>> selector) where TModel : class
    {
        var propertyName = (selector.Body as MemberExpression)?.Member?.Name ?? "UnknownProperty";
        
        validator.RuleFor(selector)
            .NotNull()
            .WithMessage($"{propertyName} is required")
            .SetValidator(new GpuIntegerTypeRestrictionsValidator(propertyName));
    }
}
