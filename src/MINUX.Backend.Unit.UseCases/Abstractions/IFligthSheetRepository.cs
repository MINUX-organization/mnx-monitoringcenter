using MINUX.Backend.Unit.Core;

namespace MINUX.Backend.Unit.UseCases.Abstractions;

public interface IFlightSheetRepository
{
    /// <summary>
    /// Получить список всех полётных листов
    /// </summary>
    /// <returns> Асинхронный список полётных листов </returns>
    public IAsyncEnumerable<FlightSheet> GetAll();

    /// <summary>
    /// Добавить полётный лист
    /// </summary>
    /// <param name="flightSheet"> Полётный лист </param>
    public Task Add(FlightSheet flightSheet);

    /// <summary>
    /// Применить полётный лист ко всем видеокартам в ферме
    /// </summary>
    /// <param name="flightSheetId"> Идентификатор полётного листа </param>
    public Task Apply(Guid flightSheetId);

    /// <summary>
    /// Обновить данные полётного листа
    /// </summary>
    /// <param name="flightSheet"> Новый полётный лист </param>
    public Task Update(FlightSheet flightSheet);

    /// <summary>
    /// Удалить полёный лист
    /// </summary>
    /// <param name="flightSheetId"> Идентификатор </param>
    public Task Remove(Guid flightSheetId);
}