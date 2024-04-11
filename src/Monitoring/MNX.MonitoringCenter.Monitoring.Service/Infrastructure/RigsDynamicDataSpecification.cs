namespace MNX.MonitoringCenter.Monitoring.Service.Infrastructure;

/// <summary>
/// Спецификация для динамических данных ригов.
/// </summary>
public struct RigsDynamicDataSpecification
{
    /// <summary>
    /// Отслеживаемая монета.
    /// </summary>
    public string? ObservableCoin { get; set; }

    /// <summary>
    /// Строка поиска для ригов.
    /// </summary>
    public string? RigsSearchString { get; set; }

    /// <summary>
    /// Строка для фильтрации.
    /// </summary>
    public string? FilterString { get; private set; }

    /// <summary>
    /// Аргументы для строки фильтрации.
    /// </summary>
    public string[]? FilterArguments { get; private set; }

    /// <summary>
    /// Задать фильтр.
    /// </summary>
    /// <param name="filterString"> Строка для фильтрации. </param>
    /// <param name="filterArguments"> Аргументы для строки фильтрации. </param>
    public void SetFilter(string filterString, string[] filterArguments)
    {
        FilterString = filterString;
        FilterArguments = filterArguments;
    }

    /// <summary>
    /// Сбросить фильтр.
    /// </summary>
    public void ResetFilter()
    {
        FilterString = null;
        FilterArguments = null;
    }
}
