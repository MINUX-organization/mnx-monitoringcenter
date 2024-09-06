using MNX.MonitoringCenter.Management.Core;

namespace MNX.MonitoringCenter.Management.UseCases.FlightSheet;

public interface IFlightSheetRepository
{
    /// <summary>
    /// Получить список всех полётных листов
    /// </summary>
    /// <returns> Асинхронный список полётных листов </returns>
    public IAsyncEnumerable<Core.FlightSheet> GetAllAvailable(Guid userId);

    /// <summary>
    /// Добавить полётный лист
    /// </summary>
    /// <param name="flightSheet"> Полётный лист </param>
    public Task Add(Core.FlightSheet flightSheet);

    /// <summary>
    /// Применить полётный лист ко всем видеокартам в ферме
    /// </summary>
    /// <param name="flightSheetId"> Идентификатор полётного листа </param>
    public Task Apply(Guid flightSheetId);

    /// <summary>
    /// Обновить данные полётного листа
    /// </summary>
    /// <param name="flightSheet"> Новый полётный лист </param>
    public Task Update(Core.FlightSheet flightSheet);

    /// <summary>
    /// Удалить полётный лист
    /// </summary>
    /// <param name="flightSheetId"> Идентификатор </param>
    public Task Remove(Guid flightSheetId);
}