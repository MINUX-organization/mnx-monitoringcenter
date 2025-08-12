using MNX.MonitoringCenter.Management.Contracts.Miner;
using MNX.MonitoringCenter.Management.UseCases.Mining.Miner.Commands.Models;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Miner;

using Miner = Core.Mining.Miner.Miner;

/// <summary>
/// Интерфейс маппера сущности <see cref="Miner"/> и его моделей.
/// </summary>
public interface IMinerMapper
{
    /// <summary>
    /// Преобразовать <see cref="MinerInputModel"/> в <see cref="Miner"/>
    /// с использованием пользовательского идентификатора.
    /// </summary>
    /// <param name="model"> Модель данных. </param>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <returns> Новый экземпляр <see cref="Miner"/>. </returns>
    Miner MapToCoreEntity(MinerInputModel model, Guid userId);

    /// <summary>
    /// Преобразовать <see cref="Miner"/> в <see cref="MinerModel"/>.
    /// </summary>
    /// <param name="entity"> Модель данных. </param>
    /// <returns> Новый экземпляр <see cref="MinerModel"/>. </returns>
    MinerModel MapToModel(Miner entity);
}
