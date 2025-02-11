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
    /// <param name="algorithms"> Сущности алгоритмов. </param>
    /// <param name="specification"> Спецификация. </param>
    /// <returns> Доступные алгоритмы. </returns>
    internal static IQueryable<Algorithm> Available(
        this IQueryable<Algorithm> algorithms, Specification specification)
    {
        return algorithms.Where(x => x.UserId == specification.UserId || x.UserId == null);
    }

    /// <summary>
    /// Получить доступные алгоритмы.
    /// </summary>
    /// <param name="algorithms"> Сущности алгоритмов. </param>
    /// <param name="specification"> Спецификация. </param>
    /// <returns> Доступные алгоритмы. </returns>
    internal static IQueryable<Algorithm> Available(
        this IQueryable<Algorithm> algorithms, Guid userId)
    {
        return algorithms.Where(x => x.UserId == userId || x.UserId == null);
    }

    /// <summary>
    /// Поиск алгоритмов по подстроке названия.
    /// </summary>
    /// <param name="algorithms"> Сущности алгоритмов. </param>
    /// <param name="specification"> Спецификация. </param>
    /// <returns> Отфильтрованный запрос. </returns>
    internal static IQueryable<Algorithm> Search(
        this IQueryable<Algorithm> algorithms, Specification specification)
    {
        if (string.IsNullOrWhiteSpace(specification.SearchSubString))
        {
            return algorithms.Where(x => EF.Functions.Like(
                x.Name, $"%{specification.SearchSubString}%"));
        }

        return algorithms;
    }
}