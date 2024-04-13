using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Monitoring.Core;
using MNX.MonitoringCenter.Monitoring.UseCases.Queries;
using System.Linq.Dynamic;

namespace MNX.MonitoringCenter.Monitoring.DataAccess.Repositories;

/// <summary>
/// Набор методов для фильтрации ригов, исходя из спецификации.
/// </summary>
internal static class RigSpecification
{
    /// <summary>
    /// Получить доступные пользователю риги.
    /// </summary>
    /// <param name="rigs"> Риги. </param>
    /// <param name="specification"> Спецификация. </param>
    /// <returns> Доступные риги. </returns>
    internal static IQueryable<Rig> Available(this IQueryable<Rig> rigs, Specification specification)
    {
        return rigs.Where(rig => rig.UserId == specification.UserId);
    }

    /// <summary>
    /// Отфильтровать коллекцию ригов.
    /// </summary>
    /// <param name="rigs"> Риги. </param>
    /// <param name="specification"> Спецификация. </param>
    /// <returns> Отфильтрованные риги. </returns>
    internal static IQueryable<Rig> Filter(this IQueryable<Rig> rigs, Specification specification)
    {
        if (!string.IsNullOrEmpty(specification.SearchString))
        {
            rigs = rigs.Search(specification.SearchString);
        }

        if (!string.IsNullOrEmpty(specification.FilterString) && specification.FilterArguments is not null)
        {
            rigs = rigs.Where(specification.FilterString, specification.FilterArguments);
        }
        
        return rigs;
    }

    /// <summary>
    /// Найти риги, исходя из строки поиска.
    /// </summary>
    /// <param name="rigs"> Риги. </param>
    /// <param name="searchString"> Строка поиска. </param>
    /// <returns> Найденные риги. </returns>
    private static IQueryable<Rig> Search(this IQueryable<Rig> rigs, string searchString)
    {
        return rigs.Where(item => EF.Functions.Like(item.Name, $"%{searchString}%"));
    }
}
