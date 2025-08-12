using MNX.MonitoringCenter.Management.Contracts.Overclocking.Gpu;
using MNX.MonitoringCenter.Management.Core.Overclocking;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Overclocking.Model;

/// <summary>
/// Интерфейс полиморфного маппера сущностей <see cref="IFanOverclocking"/> и <see cref="IFanOverclockingModel"/>.
/// </summary>
/// <typeparam name="TModel"> Модель контрактов <see cref="IFanOverclockingModel"/>. </typeparam>
/// <typeparam name="TEntity"> Модель ядра <see cref="IFanOverclocking"/>. </typeparam>
public interface IFanOverclockingModelMapper<TModel, TEntity>
    where TModel : IFanOverclockingModel
    where TEntity : IFanOverclocking
{
    /// <summary>
    /// Преобразовать реализацию модели <see cref="IFanOverclockingModel"/> в <see cref="IFanOverclocking"/>.
    /// </summary>
    /// <param name="model"> Модель данных. </param>
    /// <returns> Новый экземпляр реализации <see cref="IFanOverclocking"/>. </returns>
    IFanOverclocking MapToCoreEntity(TModel model);

    /// <summary>
    /// Преобразовать реализацию модели <see cref="IFanOverclockingModel"/> в <see cref="IFanOverclocking"/>.
    /// </summary>
    /// <param name="model"> Модель данных. </param>
    /// <param name="originalId"> Идентификатор оригинальной модели. </param>
    /// <returns> Новый экземпляр реализации <see cref="IFanOverclocking"/>. </returns>
    IFanOverclocking MapToCoreEntity(TModel model, Guid originalId);

    /// <summary>
    /// Преобразовать реализацию модели <see cref="IOverclocking"/> в <see cref="IFanOverclockingModel"/>.
    /// </summary>
    /// <param name="entity"> Модель данных. </param>
    /// <returns> Новый экземпляр реализации <see cref="IFanOverclockingModel"/>. </returns>
    IFanOverclockingModel MapToModel(TEntity entity);
}
