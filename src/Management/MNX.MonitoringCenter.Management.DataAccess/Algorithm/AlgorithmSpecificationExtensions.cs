using System.Linq.Dynamic.Core;

namespace MNX.MonitoringCenter.Management.DataAccess.Algorithm;

using Algorithm = Core.Mining.Algorithm;

/// <summary>
/// Расширения для обработки алгоритмов.
/// </summary>
internal static class AlgorithmSpecificationExtensions
{
    /// <summary>
    /// Получить доступные пользователю алгоритмы.
    /// </summary>
    /// <param name="algorithms"> Сущности алгоритмов. </param>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <returns> Доступные алгоритмы. </returns>
    internal static IQueryable<Algorithm> Available(
        this IQueryable<Algorithm> algorithms, Guid userId)
    {
        return algorithms.Where(x => x.UserId == userId || x.UserId == null);
    }
}