using FluentValidation;
using System.Linq.Expressions;

namespace MNX.MonitoringCenter.Management.UseCases.Overclocking.Validation;

/// <summary>
/// Базовый класс валидатора параметров разгона.
/// </summary>
/// <typeparam name="T"> Тип разгона. </typeparam>
public abstract class OverclockingValidatorBase<T> : AbstractValidator<T>
{
    /// <summary>
    /// Правило, определяющее значение границ,
    /// в которых может находиться параметр валидируемого разгона.
    /// </summary>
    /// <param name="property"> Валидируемое свойство. </param>
    /// <param name="propertyName"> Наименование валидируемого свойства. </param>
    /// <param name="min"> Минимально допустимая граница значения свойства. </param>
    /// <param name="max"> Максимально допустимая граница значения свойства. </param>
    protected void AddRangeValidatorRule(Expression<Func<T, int>> property,
                                         string propertyName,
                                         int? min,
                                         int? max)
    {
        if (min.HasValue)
        {
            RuleFor(property)
            .GreaterThanOrEqualTo(min.Value)
            .WithMessage(model =>
            {
                var current = property.Compile()(model);
                return GreaterThanOrEqualMessage(propertyName, min, current);
            });
        }

        if (max.HasValue)
        {
            RuleFor(property)
            .LessThanOrEqualTo(max.Value)
            .WithMessage(model =>
            {
                var current = property.Compile()(model);
                return LessThanOrEqualMessage(propertyName, max, current);
            });
        }
    }

    private string GreaterThanOrEqualMessage(string targetOverclockingParam, int? targetRestrictionsParam, int currentOverclockingParam)
        => $"{targetOverclockingParam} must be greater or equal than {targetRestrictionsParam}. Current value is {currentOverclockingParam}";

    private string LessThanOrEqualMessage(string targetOverclockingParam, int? targetRestrictionsParam, int currentOverclockingParam)
        => $"{targetOverclockingParam} must be less or equal than {targetRestrictionsParam}. Current value is {currentOverclockingParam}";
}
