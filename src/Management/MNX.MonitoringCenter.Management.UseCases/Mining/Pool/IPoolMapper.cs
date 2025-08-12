using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Management.UseCases.Mining.Pool.Commands.AddPool;
using MNX.MonitoringCenter.Management.UseCases.Mining.Pool.Commands.EditPool;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Pool;

using Pool = Core.Mining.Pool;

/// <summary>
/// Интерфейс маппера сущности <see cref="Pool"/> и его моделей.
/// </summary>
public interface IPoolMapper
{
    /// <summary>
    /// Преобразовать сущность <see cref="AddPoolCommand"/> в <see cref="Pool"/>.
    /// </summary>
    /// <param name="model"> Модель данных. </param>
    /// <returns> Новый экземпляр <see cref="Pool"/>. </returns>
    Pool MapToCoreEntity(AddPoolCommand model);

    /// <summary>
    /// Преобразовать сущность <see cref="EditPoolCommand"/> в <see cref="Pool"/>.
    /// </summary>
    /// <param name="model"> Модель данных. </param>
    /// <returns> Новый экземпляр <see cref="Pool"/>. </returns>
    Pool MapToCoreEntity(EditPoolCommand model);

    /// <summary>
    /// Преобразовать сущность <see cref="Pool"/> в <see cref="PoolModel"/>.
    /// </summary>
    /// <param name="model"> Модель данных. </param>
    /// <returns> Новый экземпляр <see cref="PoolModel"/>. </returns>
    PoolModel MapToModel(Pool entity);
}
