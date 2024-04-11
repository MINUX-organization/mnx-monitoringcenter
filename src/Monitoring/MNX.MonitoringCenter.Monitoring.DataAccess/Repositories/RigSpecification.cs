using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Monitoring.Core;
using MNX.MonitoringCenter.Monitoring.UseCases.Queries;
using System.Linq.Dynamic;

namespace MNX.MonitoringCenter.Monitoring.DataAccess.Repositories;

internal static class RigSpecification
{
    internal static IQueryable<Rig> Available(this IQueryable<Rig> items, Specification specification)
    {
        return items.Where(rig => rig.UserId == specification.UserId);
    }

    internal static IQueryable<Rig> Filter(this IQueryable<Rig> items, Specification specification)
    {
        if (!string.IsNullOrEmpty(specification.SearchString))
        {
            items = items.Search(specification.SearchString);
        }

        if (!string.IsNullOrEmpty(specification.FilterString))
        {
            items = items.Where(specification.FilterString, specification.FilterArguments);
        }
        
        return items;
    }

    private static IQueryable<Rig> Search(this IQueryable<Rig> items, string searchString)
    {
        return items.Where(item => EF.Functions.Like(item.Name, $"%{searchString}%"));
    }
}
