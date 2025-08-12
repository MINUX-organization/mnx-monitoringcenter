using MNX.MonitoringCenter.Management.Contracts.Overclocking;
using MNX.MonitoringCenter.Management.Core.Overclocking;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Infrastructure.Overclocking.Model;

/// <summary>
/// Адаптер для полиморфного маппера Сущностей <see cref="IOverclockingModel"/> и <see cref="IOverclocking"/>.
/// </summary>
public interface IOverclockingModelMappingAdapter
{
    /// <summary>
    /// Преобразует модель <see cref="IOverclockingModel"/> в сущность <see cref="IOverclocking"/>.
    /// </summary>
    /// <param name="model"> Модель данных. </param>
    /// <returns> Сконструированная сущность <see cref="IOverclocking"/>. </returns>
    IOverclocking MapToCoreEntity(IOverclockingModel model);

    /// <summary>
    /// Преобразует модель <see cref="IOverclockingModel"/> в сущность <see cref="IOverclocking"/>
    /// с использованием оригинальной сущности <see cref="IOverclocking"/>.
    /// </summary>
    /// <param name="model"> Модель данных. </param>
    /// <returns> Сконструированная сущность <see cref="IOverclocking"/>. </returns>
    IOverclocking MapToCoreEntity(IOverclockingModel model, IOverclocking original);

    /// <summary>
    /// Преобразует модель <see cref="IOverclocking"/> в сущность <see cref="IOverclockingModel"/>.
    /// </summary>
    /// <param name="entity"> Модель данных. </param>
    /// <returns> Сконструированная сущность <see cref="IOverclockingModel"/>. </returns>
    IOverclockingModel MapToModel(IOverclocking entity);

    /// <summary>
    /// Тип реализации <see cref="IOverclockingModel"/>.
    /// </summary>
    Type ModelType { get; }

    /// <summary>
    /// Тип реализации <see cref="IOverclocking"/>.
    /// </summary>
    Type EntityType { get; }
}
