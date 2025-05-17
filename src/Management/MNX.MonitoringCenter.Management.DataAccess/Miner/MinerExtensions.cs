using System.Linq.Dynamic.Core;
using MNX.MonitoringCenter.Management.Core.Mining.Miner.Enums;
using MNX.MonitoringCenter.Management.UseCases;

namespace MNX.MonitoringCenter.Management.DataAccess.Miner;

using Miner = Core.Mining.Miner.Miner;

/// <summary>
/// Класс с расширениями функций <see cref="MinerRepository"/>.
/// </summary>
internal static class MinerExtensions
{
    /// <summary>
    /// Фильтровать сущности по спецификации.
    /// </summary>
    /// <param name="entities"> Запрос на получение сущностей. </param>
    /// <param name="specification"> Спецификация. </param>
    /// <returns> Запрос на получение отфильтрованных сущностей. </returns>
    internal static IQueryable<Miner> Filter(
        this IQueryable<Miner> entities, Specification specification)
    {
        entities = entities.Where(miner => miner.Type == MinerTypeEnum.Integrated ||
                                           miner.OwnerId == specification.UserId);

        if (!string.IsNullOrWhiteSpace(specification.FilterString) &&
            specification.FilterParameters is not null)
        {
            return entities
                .Where(specification.FilterString, specification.FilterParameters);
        }

        return entities;
    }
}