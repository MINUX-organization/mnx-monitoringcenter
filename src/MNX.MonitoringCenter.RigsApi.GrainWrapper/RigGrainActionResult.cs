using MNX.MonitoringCenter.RigsApi.Core.DomainEvents;

namespace MNX.MonitoringCenter.RigsApi.GrainWrapper;

/// <summary>
/// Результат выполнения действия над ригом.
/// </summary>
public class RigGrainActionResult
{
    private IReadOnlyCollection<BaseDomainEvent> _events = Array.Empty<BaseDomainEvent>();

    private IReadOnlyCollection<string> _errors = Array.Empty<string>();

    /// <summary>
    /// Признак успешности.
    /// </summary>
    public bool IsSuccess { get; }

    ///
    private RigGrainActionResult(BaseDomainEvent[] events)
    {
        IsSuccess = true;
        _events = events;
    }

    ///
    private RigGrainActionResult(string[] errors, BaseDomainEvent[]? events = null)
    {
        IsSuccess = false;
        _events = events ?? Array.Empty<BaseDomainEvent>();
        _errors = errors;
    }

    /// <summary>
    /// Создать успешный результат.
    /// </summary>
    /// <param name="events"> События. </param>
    /// <returns> Успешный результат действия над ригом. </returns>
    public static RigGrainActionResult Success(params BaseDomainEvent[] events)
    {
        return new RigGrainActionResult(events);
    }

    /// <summary>
    /// Создать неудачный результат.
    /// </summary>
    /// <param name="errors"> Ошибки. </param>
    /// <returns> Неудачный результат действия над ригом. </returns>
    public static RigGrainActionResult Error(string[] errors, params BaseDomainEvent[] events)
    {
        return new RigGrainActionResult(errors, events);
    }

    /// <summary>
    /// Получить доменные события.
    /// </summary>
    /// <returns> Доменные события. </returns>
    public IReadOnlyCollection<BaseDomainEvent> GetDomainEvents() => _events;

    /// <summary>
    /// Получить ошибки.
    /// </summary>
    /// <returns> Ошибки. </returns>
    public IReadOnlyCollection<string> GetErrors() => _errors;
}
