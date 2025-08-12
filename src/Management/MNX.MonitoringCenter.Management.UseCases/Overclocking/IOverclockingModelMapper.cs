using MNX.MonitoringCenter.Management.Contracts.Overclocking;
using MNX.MonitoringCenter.Management.Core.Overclocking;

namespace MNX.MonitoringCenter.Management.UseCases.Overclocking;

/// <summary>
/// Интерфейс полиморфного маппера сущностей <see cref="IOverclocking"/> и <see cref="IOverclockingModel"/>.
/// </summary>
/// <typeparam name="TModel"> Модель контрактов <see cref="IOverclockingModel"/>. </typeparam>
/// <typeparam name="TEntity"> Модель ядра <see cref="IOverclocking"/>. </typeparam>
public interface IOverclockingModelMapper<TModel, TEntity>
    where TModel : IOverclockingModel
    where TEntity : IOverclocking
{
    /// <summary>
    /// Преобразовать реализацию модели <see cref="IOverclockingModel"/> в <see cref="IOverclocking"/>.
    /// </summary>
    /// <param name="model"> Модель данных. </param>
    /// <returns> Новый экземпляр реализации <see cref="IOverclocking"/>. </returns>
    IOverclocking MapToCoreEntity(TModel model);

    /// <summary>
    /// Преобразовать реализацию модели <see cref="IOverclockingModel"/> в <see cref="IOverclocking"/>.
    /// </summary>
    /// <param name="model"> Модель данных. </param>
    /// <param name="original"> Оригинальная модель. </param>
    /// <returns> Новый экземпляр реализации <see cref="IOverclocking"/>. </returns>
    IOverclocking MapToCoreEntity(TModel model, TEntity original);

    /// <summary>
    /// Преобразовать реализацию модели <see cref="IOverclocking"/> в <see cref="IOverclockingModel"/>.
    /// </summary>
    /// <param name="entity"> Модель данных. </param>
    /// <returns> Новый экземпляр реализации <see cref="IOverclockingModel"/>. </returns>
    IOverclockingModel MapToModel(TEntity entity);
}
