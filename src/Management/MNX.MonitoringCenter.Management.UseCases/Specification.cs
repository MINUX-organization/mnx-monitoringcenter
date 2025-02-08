namespace MNX.MonitoringCenter.Management.UseCases;

/// <summary>
/// Тип сортировки.
/// </summary>
public enum SortType
{
    /// <summary>
    /// По алфавиту.
    /// </summary>
    Alphabetically
}

/// <summary>
/// Спецификация.
/// </summary>
public readonly struct Specification(Guid userId)
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; } = userId;

    /// <summary>
    /// Строка фильтрации.
    /// </summary>
    public string? FilterString { get; }

    /// <summary>
    /// Параметры фильтрации.
    /// </summary>
    public object[]? FilterParameters { get; }

    /// <summary>
    /// Подстрока поиска.
    /// </summary>
    public string SearchSubString { get; } = string.Empty;

    public Specification(Guid userId, string subString) : this(userId)
    {
        SearchSubString = subString;
    }

    public Specification(Guid userId, string filterString, object[] filterParameters)
        : this(userId)
    {
        FilterString = filterString;
        FilterParameters = filterParameters;
    }
}