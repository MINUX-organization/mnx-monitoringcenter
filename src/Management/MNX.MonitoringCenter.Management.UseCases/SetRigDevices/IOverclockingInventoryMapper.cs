using MNX.MonitoringCenter.Management.Core.Overclocking;

namespace MNX.MonitoringCenter.Management.UseCases.SetRigDevices;

using OverclockingInventory = Inventory.Contracts.Devices.Overclocking;

/// <summary>
/// Интерфейс маппера сущностей <see cref="OverclockingInventory"/>
/// и <see cref="IOverclocking"/>.
/// </summary>
/// <typeparam name="TModel"> Сущность <see cref="OverclockingInventory"/>. </typeparam>
/// <typeparam name="TEntity"> Сущность <see cref="IOverclocking"/>. </typeparam>
public interface IOverclockingInventoryMapper<TModel, TEntity>
    where TModel : OverclockingInventory
    where TEntity : IOverclocking
{
    /// <summary>
    /// Преобразовать реализации сущности <see cref="OverclockingInventory"/>
    /// в реализации сущности <see cref="IOverclocking"/>.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    IOverclocking MapToCoreEntity(TModel model);

    /// <summary>
    /// Преобразовать реализации сущности <see cref="IOverclocking"/>
    /// в реализации сущности <see cref="OverclockingInventory"/>
    /// </summary>
    /// <param name="entity"> Модель маппинга. </param>
    /// <returns>
    /// Новый экземпляр реализации <see cref="OverclockingInventory"/>.
    /// </returns>
    OverclockingInventory MapToModel(TEntity entity);
}
