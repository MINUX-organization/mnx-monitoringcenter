using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.UseCases.SetRigDevices;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Infrastructure.Overclocking.Inventory;

using OverclockingInventory = MonitoringCenter.Inventory.Contracts.Devices.Overclocking;

/// <summary>
/// Реестр реализаций маппера <see cref="IOverclockingInventoryMapper{TModel, TEntity}"/>.
/// </summary>
/// <remarks>
/// Данный реестр используется для маршрутизации маппера к конкретной реализации,
/// в зависимости от типа преобразуемой сущности.
/// </remarks>
public class OverclockingInventoryMapperRegistry
{
    private readonly Dictionary<Type, IOverclockingInventoryMappingAdapter> _modelMappers = [];
    private readonly Dictionary<Type, IOverclockingInventoryMappingAdapter> _entityMappers = [];

    /// <summary>
    /// Зарегистрировать все мапперы <see cref="IOverclockingInventoryMapper{TModel, TEntity}"/>.
    /// </summary>
    /// <param name="mapper"> Регистрируемый маппер. </param>
    public void Register<TModel, TEntity>(IOverclockingInventoryMapper<TModel, TEntity> mapper)
        where TModel : OverclockingInventory
        where TEntity : IOverclocking
    {
        var adapter = new OverclockingInventoryMappingAdapter<TModel, TEntity>(mapper);
        _modelMappers[typeof(TModel)] = adapter;
        _entityMappers[typeof(TEntity)] = adapter;
    }

    /// <summary>
    /// Преобразовать сущность <see cref="OverclockingInventory"/>
    /// в сущность <see cref="IOverclocking"/>.
    /// </summary>
    /// <param name="model"> Модель данных. </param>
    /// <returns>
    /// Сконструированная сущность <see cref="IOverclocking"/>,
    /// на основе представленной сущности <see cref="OverclockingInventory"/>.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Возникает, если для типа модели не зарегистрирован соответствующий маппер.
    /// </exception>
    public IOverclocking MapToCore(OverclockingInventory model)
    {
        var type = model.GetType();
        if (!_modelMappers.TryGetValue(type, out var adapter))
            throw new InvalidOperationException($"Mapper not found for model type: {type.Name}");

        return adapter.MapToCore(model);
    }

    /// <summary>
    /// Преобразовать сущность <see cref="IOverclocking"/>
    /// в сущность <see cref="OverclockingInventory"/>.
    /// </summary>
    /// <param name="model"> Модель данных. </param>
    /// <returns>
    /// Сконструированная сущность <see cref="OverclockingInventory"/>,
    /// на основе представленной сущности <see cref="IOverclocking"/>.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Возникает, если для типа модели не зарегистрирован соответствующий маппер.
    /// </exception>
    public OverclockingInventory MapToModel(IOverclocking entity)
    {
        var type = entity.GetType();
        if (!_entityMappers.TryGetValue(type, out var adapter))
            throw new InvalidOperationException($"Mapper not found for model type: {type.Name}");

        return adapter.MapToModel(entity);
    }
}
