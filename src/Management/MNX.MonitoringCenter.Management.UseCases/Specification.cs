namespace MNX.MonitoringCenter.Management.UseCases;

/// <summary>
/// Спецификация.
/// </summary>
public readonly struct Specification
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; }

    /// <summary>
    /// Идентификаторы запрашиваемых объектов.
    /// </summary>
    public Guid[]? Ids { get; }

    /// <summary>
    /// Строка фильтрации.
    /// </summary>
    public string? FilterString { get; }

    /// <summary>
    /// Параметры фильтрации.
    /// </summary>
    public string[]? FilterParameters { get; }

    public Specification(Guid userId, Guid[]? ids = null)
    {
        UserId = userId;
        Ids = ids;
    }

    public Specification(Guid userId, Guid[]? ids, string filterString, string[] filterParameters)
        : this(userId, ids)
    {
        FilterString = filterString;
        FilterParameters = filterParameters;
    }
}