using System.Linq.Dynamic.Core;
using MNX.MonitoringCenter.Management.UseCases;
using MNX.MonitoringCenter.Management.Core.Mining.Miner.Enums;

namespace MNX.MonitoringCenter.Management.DataAccess.Miner;

using Miner = Core.Mining.Miner.Miner;

internal static class MinerExtensions
{
    internal static IQueryable<Miner> Filter(
        this IQueryable<Miner> entities, Specification specification)
    {
        entities = entities.Where(miner =>
            miner.Type == MinerTypeEnum.Integrated || miner.OwnerId == specification.UserId);

        if (!string.IsNullOrWhiteSpace(specification.FilterString) &&
            specification.FilterParameters is not null)
        {
            return entities
                .Where(specification.FilterString, specification.FilterParameters);
        }

        return entities;
    }
}