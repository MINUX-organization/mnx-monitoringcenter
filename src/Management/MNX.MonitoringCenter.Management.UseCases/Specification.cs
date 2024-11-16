namespace MNX.MonitoringCenter.Management.UseCases;

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

    public Specification(Guid userId, string filterString, object[] filterParameters)
        : this(userId)
    {
        FilterString = filterString;
        FilterParameters = filterParameters;
    }
}