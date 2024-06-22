using MNX.MonitoringCenter.Monitoring.Core;

namespace MNX.MonitoringCenter.Monitoring.UseCases.Abstractions;

/// <summary>
/// Репозиторий для разгонов видеокарт.
/// </summary>
public interface IOverclockingRepository
{
    /// <summary>
    /// Получить разгон по идентификатору видеокарты.
    /// </summary>
    /// <param name="id"> Идентификатор видеокарты. </param>
    /// <returns> Разгон видеокарты. </returns>
    Task<Overclocking?> GetOverclockingById(Guid id);

    /// <summary>
    /// Задать разгон по идентификатору видеокарты.
    /// </summary>
    /// <param name="id"> Идентификатор видеокарты. </param>
    /// <param name="overclocking"> Разгон. </param>
    Task SetOverclockingById(Guid id, Overclocking overclocking);
}
