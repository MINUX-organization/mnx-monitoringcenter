namespace MNX.MonitoringCenter.Monitoring.UseCases.Queries;

/// <summary>
/// Спецификация.
/// </summary>
public class Specification
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public long UserId { get; }

    /// <summary>
    /// Строка поиска для ригов.
    /// </summary>
    public string? SearchString { get; }

    /// <summary>
    /// Строка для фильтрации.
    /// </summary>
    public string? FilterString { get; }

    /// <summary>
    /// Аргументы для строки фильтрации.
    /// </summary>
    public string[]? FilterArguments { get; }

    public Specification(long userId,
                         string? searchString = null,
                         string? filterString = null,
                         string[]? filterArguments = null)
    {
        UserId = userId;
        SearchString = searchString;

        if (filterString is not null && filterArguments is null ||
            filterString is null && filterArguments is not null)
        {
            throw new ArgumentNullException("Один из параметров для фильтра есть, а другого нет.");
        }

        FilterString = filterString;
        FilterArguments = filterArguments;
    }
}
