using MNX.MonitoringCenter.Management.Contracts.Overclocking;
using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.UseCases.Overclocking;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Infrastructure.Overclocking.Model;

/// <summary>
/// Реестр реализаций маппера <see cref="IOverclockingModelMapper{TModel, TEntity}"/>.
/// </summary>
/// <remarks>
/// Данный реестр используется для маршрутизации маппера к конкретной реализации,
/// в зависимости от типа преобразуемой сущности.
/// </remarks>
public class OverclockingModelMapperRegistry
{
    private readonly Dictionary<Type, IOverclockingModelMappingAdapter> _modelMappers = [];
    private readonly Dictionary<Type, IOverclockingModelMappingAdapter> _entityMappers = [];

    /// <summary>
    /// Зарегистрировать все мапперы <see cref="IOverclockingModelMapper{TModel, TEntity}"/>.
    /// </summary>
    /// <param name="mapper"> Регистрируемый маппер. </param>
    public void Register<TModel, TEntity>(IOverclockingModelMapper<TModel, TEntity> mapper)
        where TModel : IOverclockingModel
        where TEntity : IOverclocking
    {
        var adapter = new OverclockingModelMappingAdapter<TModel, TEntity>(mapper);
        _modelMappers[typeof(TModel)] = adapter;
        _entityMappers[typeof(TEntity)] = adapter;
    }

    /// <summary>
    /// Преобразовать сущность <see cref="IOverclockingModel"/>
    /// в сущность <see cref="IOverclocking"/>.
    /// </summary>
    /// <param name="model"> Модель данных. </param>
    /// <returns>
    /// Сконструированная сущность <see cref="IFanOverclocking"/>,
    /// на основе предоставленной сущности <see cref="IOverclockingModel"/>.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Возникает, если для типа модели не зарегистрирован соответствующий маппер.
    /// </exception>
    public IOverclocking MapToCoreEntity(IOverclockingModel model)
    {
        var type = model.GetType();
        if (!_modelMappers.TryGetValue(type, out var adapter))
            throw new InvalidOperationException($"Mapper not found for model type: {type.Name}");

        return adapter.MapToCoreEntity(model);
    }

    /// <summary>
    /// Преобразовать модель <see cref="IOverclockingModel"/> в сущность <see cref="IOverclocking"/>
    /// с использованием оригинальной сущности <see cref="IOverclocking"/>.
    /// </summary>
    /// <param name="model"> Модель данных. </param>
    /// <param name="original"> Оригинальная модель. </param>
    /// <returns>
    /// Сконструированная сущность <see cref="IOverclocking"/>,
    /// на основе предоставленной сущности <see cref="IOverclockingModel"/>.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Возникает, если для типа модели не зарегистрирован соответствующий маппер.
    /// </exception>
    public IOverclocking MapToCoreEntity(IOverclockingModel model, IOverclocking original)
    {
        var type = model.GetType();
        if (!_modelMappers.TryGetValue(type, out var adapter))
            throw new InvalidOperationException($"Mapper not found for model type: {type.Name}");

        return adapter.MapToCoreEntity(model, original);
    }

    /// <summary>
    /// Преобразовать модель <see cref="IOverclocking"/>
    /// в сущность <see cref="IOverclockingModel"/>.
    /// </summary>
    /// <param name="entity"> Модель данных. </param>
    /// <returns>
    /// Сконструированная сущность <see cref="IOverclockingModel"/>,
    /// на основе предоставленной сущности <see cref="IOverclocking"/>.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Возникает, если для типа модели не зарегистрирован соответствующий маппер.
    /// </exception>
    public IOverclockingModel MapToModel(IOverclocking entity)
    {
        var type = entity.GetType();
        if (!_entityMappers.TryGetValue(type, out var adapter))
            throw new InvalidOperationException($"Mapper not found for entity type: {type.Name}");

        return adapter.MapToModel(entity);
    }
}
