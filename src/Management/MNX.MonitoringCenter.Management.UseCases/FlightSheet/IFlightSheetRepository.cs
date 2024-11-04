namespace MNX.MonitoringCenter.Management.UseCases.FlightSheet;

using FlightSheet = Core.FlightSheet.FlightSheet;

/// <summary>
/// Репозиторий для доступа к полётным листам.
/// </summary>
public interface IFlightSheetRepository
{
    /// <summary>
    /// Получить список всех доступных полётных листов.
    /// </summary>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <returns> Асинхронный список полётных листов. </returns>
    IAsyncEnumerable<FlightSheet> GetAllAvailable(Guid userId);

    /// <summary>
    /// Получить полётный лист по идентификатору.
    /// </summary>
    /// <param name="flightSheetId"> Идентификатор полётного листа. </param>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns> Полётный лист. </returns>
    Task<FlightSheet?> GetAvailableById(Guid flightSheetId, Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Получить признак существования полётного листа с переданным названием.
    /// </summary>
    /// <param name="name"> Название. </param>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns>
    /// <see langword="true"/>, если существует, иначе <see langword="false"/>.
    /// </returns>
    Task<bool> ExistsAvailable(string name, Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Получить признак существования полётного листа с переданным идентификатором.
    /// </summary>
    /// <param name="id"> Идентификатор. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns>
    /// <see langword="true"/>, если существует, иначе <see langword="false"/>.
    /// </returns>
    Task<bool> Exists(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Добавить полётный лист.
    /// </summary>
    /// <param name="flightSheet"> Полётный лист. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    Task Add(FlightSheet flightSheet, CancellationToken cancellationToken);

    /// <summary>
    /// Редактировать полётный лист.
    /// </summary>
    /// <param name="flightSheet"> Новый полётный лист. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    Task Edit(FlightSheet flightSheet, CancellationToken cancellationToken);

    /// <summary>
    /// Удалить полётный лист.
    /// </summary>
    /// <param name="id"> Идентификатор полетного листа. </param>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    Task Remove(Guid id, Guid userId, CancellationToken cancellationToken);
}