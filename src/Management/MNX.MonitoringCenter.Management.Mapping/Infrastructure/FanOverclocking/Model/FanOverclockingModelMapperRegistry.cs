using MNX.MonitoringCenter.Management.Contracts.Overclocking.Gpu;
using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Overclocking.Model;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Infrastructure.FanOverclocking.Model;

/// <summary>
/// Реестр реализаций маппера <see cref="IFanOverclockingModelMapper{TModel, TEntity}"/>.
/// </summary>
/// <remarks>
/// Данный реестр используется для маршрутизации маппера к конкретной реализации,
/// в зависимости от типа преобразуемой сущности.
/// </remarks>
public class FanOverclockingModelMapperRegistry
{
    private readonly Dictionary<Type, IFanOverclockingModelMappingAdapter> _modelMappers = [];
    private readonly Dictionary<Type, IFanOverclockingModelMappingAdapter> _entityMappers = [];

    /// <summary>
    /// Зарегистрировать все мапперы <see cref="IFanOverclockingModelMapper{TModel, TEntity}"/>.
    /// </summary>
    /// <param name="mapper"> Регистрируемый маппер. </param>
    public void Register<TModel, TEntity>(IFanOverclockingModelMapper<TModel, TEntity> mapper)
        where TModel : IFanOverclockingModel
        where TEntity : IFanOverclocking
    {
        var adapter = new FanOverclockingModelMappingAdapter<TModel, TEntity>(mapper);
        _modelMappers[typeof(TModel)] = adapter;
        _entityMappers[typeof(TEntity)] = adapter;
    }

    /// <summary>
    /// Преобразует модель <see cref="IFanOverclockingModel"/> в сущность <see cref="IFanOverclocking"/>.
    /// </summary>
    /// <param name="model"> Модель данных. </param>
    /// <returns>
    /// Сконструированная сущность <see cref="IFanOverclocking"/> на основе предоставленной модели.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Возникает, если в DI для типа модели не зарегистрирован соответствующий маппер.
    /// </exception>
    public IFanOverclocking MapToCoreEntity(IFanOverclockingModel model)
    {
        var type = model.GetType();
        if (!_modelMappers.TryGetValue(type, out var adapter))
            throw new InvalidOperationException($"Mapper not found for fan model type: {type.Name}");

        return adapter.MapToCoreEntity(model);
    }

    /// <summary>
    /// Преобразует модель <see cref="IFanOverclockingModel"/> в сущность <see cref="IFanOverclocking"/>
    /// с использованием идентификатора оригинальной сущности <see cref="IFanOverclocking"/>.
    /// </summary>
    /// <param name="model"> Модель данных. </param>
    /// <param name="id"> Идентификатор оригинальной модели. </param>
    /// <returns>
    /// Сконструированная сущность <see cref="IFanOverclocking"/> на основе предоставленной модели.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Возникает, если в DI для типа модели не зарегистрирован соответствующий маппер.
    /// </exception>
    public IFanOverclocking MapToCoreEntity(IFanOverclockingModel model, Guid id)
    {
        var type = model.GetType();
        if (!_modelMappers.TryGetValue(type, out var adapter))
            throw new InvalidOperationException($"Mapper not found for fan model type: {type.Name}");

        return adapter.MapToCoreEntity(model, id);
    }

    /// <summary>
    /// Преобразует модель <see cref="IFanOverclocking"/> в сущность <see cref="IFanOverclockingModel"/>.
    /// </summary>
    /// <param name="entity"> Модель данных. </param>
    /// <returns>
    /// Сконструированная сущность <see cref="IFanOverclockingModel"/> на основе предоставленной модели.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Возникает, если в DI для типа модели не зарегистрирован соответствующий маппер.
    /// </exception>
    public IFanOverclockingModel MapToModel(IFanOverclocking entity)
    {
        var type = entity.GetType();
        if (!_entityMappers.TryGetValue(type, out var adapter))
            throw new InvalidOperationException($"Mapper not found for fan entity type: {type.Name}");

        return adapter.MapToModel(entity);
    }
}
