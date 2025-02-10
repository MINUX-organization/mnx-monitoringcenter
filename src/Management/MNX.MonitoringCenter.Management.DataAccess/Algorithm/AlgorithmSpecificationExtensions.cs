using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Management.UseCases;
using System.Linq.Dynamic.Core;

namespace MNX.MonitoringCenter.Management.DataAccess.Algorithm;

using Algorithm = Core.Mining.Algorithm;

/// <summary>
/// Расширения для обработки алгоритмов.
/// </summary>
internal static class AlgorithmSpecificationExtensions
{
    /// <summary>
    /// Получить доступные алгоритмы.
    /// </summary>
    /// <param name="devices"> Сущности алгоритмов. </param>
    /// <param name="specification"> Спецификация. </param>
    /// <returns> Доступные алгоритмы. </returns>
    internal static IQueryable<Algorithm> Avilable(
        this IQueryable<Algorithm> entities, Specification specification)
    {
        return entities.Where(x => x.UserId == specification.UserId || x.UserId == null);
    }

    /// <summary>
    /// Поиск алгоритмов по подстроке названия.
    /// </summary>
    /// <param name="entities"> Сущности алгоритмов. </param>
    /// <param name="specification"> Спецификация. </param>
    /// <returns> Отфильтрованный запрос. </returns>
    internal static IQueryable<Algorithm> Search(
        this IQueryable<Algorithm> entities, Specification specification)
    {
        if (specification.SearchSubString != string.Empty)
        {
            return entities.Where(x => EF.Functions.Like(
                x.Name, $"%{specification.SearchSubString}%"));
        }

        return entities;
    }
}