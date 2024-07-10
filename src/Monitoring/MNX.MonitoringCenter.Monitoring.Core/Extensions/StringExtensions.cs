namespace MNX.MonitoringCenter.Monitoring.Core.Extensions;

/// <summary>
/// Расширения для строки.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Преобразовать в перечисление.
    /// </summary>
    /// <typeparam name="T"> Тип перечисления. </typeparam>
    /// <param name="value"> Значение. </param>
    /// <returns> Перечисление. </returns>
    public static T ToEnum<T>(this string value)
    {
        return (T) Enum.Parse(typeof(T), value, true);
    }
}
