using MNX.MonitoringCenter.Management.Contracts.Overclocking.Gpu;
using MNX.MonitoringCenter.Management.Core.Overclocking;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Infrastructure.FanOverclocking.Model;

/// <summary>
/// Адаптер для полиморфного маппера Сущностей <see cref="IFanOverclockingModel"/> и <see cref="IFanOverclocking"/>.
/// </summary>
public interface IFanOverclockingModelMappingAdapter
{
    /// <summary>
    /// Преобразовать модель <see cref="IFanOverclockingModel"/> в сущность <see cref="IFanOverclocking"/>.
    /// </summary>
    /// <param name="model"> Модель данных. </param>
    /// <returns> Сконструированная сущность <see cref="IFanOverclocking"/>. </returns>
    IFanOverclocking MapToCoreEntity(IFanOverclockingModel model);

    /// <summary>
    /// Преобразовать модель <see cref="IFanOverclockingModel"/> в сущность <see cref="IFanOverclocking"/>.
    /// </summary>
    /// <param name="model"> Модель данных. </param>
    /// <param name="originalId"> Идентификатор оригинальной сущности. </param>
    /// <returns> Сконструированная сущность <see cref="IFanOverclocking"/>. </returns>
    IFanOverclocking MapToCoreEntity(IFanOverclockingModel model, Guid originalId);

    /// <summary>
    /// Преобразовать модель <see cref="IFanOverclocking"/> в сущность <see cref="IFanOverclockingModel"/>.
    /// </summary>
    /// <param name="entity"> Модель данных. </param>
    /// <returns> Сконструированная сущность <see cref="IFanOverclockingModel"/>. </returns>
    IFanOverclockingModel MapToModel(IFanOverclocking entity);

    /// <summary>
    /// Тип реализации <see cref="IFanOverclocking"/>.
    /// </summary>
    Type ModelType { get; }

    /// <summary>
    /// Тип реализации <see cref="IFanOverclockingModel"/>.
    /// </summary>
    Type EntityType { get; }
}
