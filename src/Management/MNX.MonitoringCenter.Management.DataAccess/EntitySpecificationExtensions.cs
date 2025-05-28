using System.Linq.Dynamic.Core;
using MNX.MonitoringCenter.Management.UseCases;

namespace MNX.MonitoringCenter.Management.DataAccess;

/// <summary>
/// Расширения для обработчики спецификации.
/// </summary>
internal static class EntitySpecificationExtensions
{
    /// <summary>
    /// Фильтровать запрашиваемые сущности.
    /// </summary>
    /// <param name="entities"> Сущности. </param>
    /// <param name="specification"> Спецификация. </param>
    /// <returns> Отфильтрованные алгоритмы. </returns>
    internal static IQueryable<T> Filter<T>(
        this IQueryable<T> entities, Specification specification)
    {
        if (!string.IsNullOrWhiteSpace(specification.FilterString) &&
            specification.FilterParameters is not null)
        {
            return entities.Where(specification.FilterString, specification.FilterParameters);
        }

        return entities;
    }
}