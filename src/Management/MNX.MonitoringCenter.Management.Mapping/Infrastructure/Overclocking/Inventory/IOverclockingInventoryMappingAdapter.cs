using MNX.MonitoringCenter.Management.Core.Overclocking;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Infrastructure.Overclocking.Inventory;

using OverclockingInventory = MonitoringCenter.Inventory.Contracts.Devices.Overclocking;

/// <summary>
/// Адаптер для полиморфного маппера Сущностей <see cref="OverclockingInventory"/> и <see cref="IOverclocking"/>.
/// </summary>
public interface IOverclockingInventoryMappingAdapter
{
    /// <summary>
    /// Преобразовать реализацию сущности <see cref="IOverclocking"/>
    /// в реализацию сущности <see cref="OverclockingInventory"/>.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    OverclockingInventory MapToModel(IOverclocking entity);

    /// <summary>
    /// Преобразовать реализацию сущности <see cref="OverclockingInventory"/>
    /// в реализацию сущности <see cref="IOverclocking"/>.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    IOverclocking MapToCore(OverclockingInventory model);

    /// <summary>
    /// Тип реализации <see cref="OverclockingInventory"/>.
    /// </summary>
    Type ModelType { get; }

    /// <summary>
    /// Тип реализации <see cref="IOverclocking"/>.
    /// </summary>
    Type EntityType { get; }
}
