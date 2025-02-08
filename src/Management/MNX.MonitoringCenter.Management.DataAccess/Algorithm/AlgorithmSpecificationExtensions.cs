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
    /// Фильтровать запрашиваемые алгоритмы 
    /// с возможностью поиска подстрок.
    /// </summary>
    /// <param name="entities"> Сущности алгоритмов. </param>
    /// <param name="specification"> Спецификация. </param>
    /// <returns> Отфильтрованный запрос. </returns>
    internal static IQueryable<Algorithm> Search(
        this IQueryable<Algorithm> entities, Specification specification)
    {
        if (specification.SearchSubString != string.Empty)
        {
            return entities
                .Where(x => (x.UserId == specification.UserId || 
                       x.UserId == null) &&
                       EF.Functions.Like(x.Name, $"%{specification.SearchSubString}%"));
        }

        return entities.Where(x => x.UserId == specification.UserId ||
                              x.UserId == null);
    }
}