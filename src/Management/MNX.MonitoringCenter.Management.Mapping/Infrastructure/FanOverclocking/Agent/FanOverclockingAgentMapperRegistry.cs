using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Overclocking.Agent;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Infrastructure.FanOverclocking.Agent;

using FanOverclockingAgent = Management.Agent.Commands.Overclocking.Fan.FanOverclocking;

/// <summary>
/// Реестр реализаций маппера <see cref="IFanOverclockingAgentMapper{TModel, TEntity}"/>.
/// </summary>
/// <remarks>
/// Данный реестр используется для маршрутизации маппера к конкретной реализации,
/// в зависимости от типа преобразуемой сущности.
/// </remarks>
public class FanOverclockingAgentMapperRegistry
{
    private readonly Dictionary<Type, IFanOverclockingAgentMappingAdapter> _modelMappers = [];
    private readonly Dictionary<Type, IFanOverclockingAgentMappingAdapter> _entityMappers = [];

    /// <summary>
    /// Зарегистрировать все мапперы <see cref="IFanOverclockingAgentMapper{TModel, TEntity}"/>.
    /// </summary>
    /// <param name="mapper"> Регистрируемый маппер. </param>
    public void Register<TModel, TEntity>(IFanOverclockingAgentMapper<TModel, TEntity> mapper)
        where TModel : FanOverclockingAgent
        where TEntity : IFanOverclocking
    {
        var adapter = new FanOverclockingAgentMappingAdapter<TModel, TEntity>(mapper);
        _modelMappers[typeof(TModel)] = adapter;
        _entityMappers[typeof(TEntity)] = adapter;
    }

    /// <summary>
    /// Преобразовать модель <see cref="IFanOverclocking"/> в сущность <see cref="FanOverclockingAgent"/>.
    /// </summary>
    /// <param name="entity"> Модель данных. </param>
    /// <returns>
    /// Сконструированная сущность <see cref="FanOverclockingAgent"/> на основе предоставленной модели.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Возникает, если в DI для типа модели не зарегистрирован соответствующий маппер.
    /// </exception>
    public FanOverclockingAgent MapToFanOverclockingModel(IFanOverclocking entity)
    {
        var type = entity.GetType();
        if (!_entityMappers.TryGetValue(type, out var adapter))
            throw new InvalidOperationException($"Mapper not found for fan model type: {type.Name}");

        return adapter.MapToModel(entity);
    }
}
