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
    /// Идентификаторы устройств.
    /// </summary>
    public Guid[]? DevicesIds { get; }

    /// <summary>
    /// Строка фильтрации.
    /// </summary>
    public string? FilterString { get; }

    /// <summary>
    /// Параметры фильтрации.
    /// </summary>
    public string[]? FilterParameters { get; }

    public Specification(Guid userId, Guid[]? devicesIds = null)
    {
        UserId = userId;
        DevicesIds = devicesIds;
    }

    public Specification(Guid userId, Guid[]? devicesIds, string filterString, string[] filterParameters)
        : this(userId, devicesIds)
    {
        FilterString = filterString;
        FilterParameters = filterParameters;
    }
}
