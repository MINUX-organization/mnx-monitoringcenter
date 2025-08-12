using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Management.UseCases.Mining.Cryptocurrency.Commands;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Cryptocurrency;

using Cryptocurrency = Core.Mining.Cryptocurrency;

/// <summary>
/// Интерфейс маппинга сущности <see cref="Cryptocurrency"/>.
/// </summary>
public interface ICryptocurrencyMapper
{
    /// <summary>
    /// Преобразовать сущность <see cref="CryptocurrencyInputModel"/> в <see cref="Cryptocurrency"/>
    /// с использованием идентификатора пользователя.
    /// </summary>
    /// <param name="model"> Модель данных. </param>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <returns> Новый экземпляр <see cref="Cryptocurrency"/>. </returns>
    Cryptocurrency MapToCoreEntity(CryptocurrencyInputModel model, Guid userId);

    /// <summary>
    /// Преобразовать сущность <see cref="Cryptocurrency"/> в <see cref="CryptocurrencyModel"/>.
    /// </summary>
    /// <param name="entity"> Модель данных. </param>
    /// <returns> Новый экземпляр <see cref="CryptocurrencyModel"/>. </returns>
    CryptocurrencyModel MapToModel(Cryptocurrency entity);
}
