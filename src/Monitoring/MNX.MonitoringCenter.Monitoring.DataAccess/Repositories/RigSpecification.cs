using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Monitoring.DataAccess.Dto;
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
    internal static IQueryable<RigDto> Available(this IQueryable<RigDto> rigs, Specification specification)
    {
        return rigs.Where(rig => rig.UserId == specification.UserId);
    }

    /// <summary>
    /// Отфильтровать коллекцию ригов.
    /// </summary>
    /// <param name="rigs"> Риги. </param>
    /// <param name="specification"> Спецификация. </param>
    /// <returns> Отфильтрованные риги. </returns>
    internal static IQueryable<RigDto> Filter(this IQueryable<RigDto> rigs, Specification specification)
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
    private static IQueryable<RigDto> Search(this IQueryable<RigDto> rigs, string searchString)
    {
        return rigs.Where(item => EF.Functions.Like(item.Name, $"%{searchString}%"));
    }
}
