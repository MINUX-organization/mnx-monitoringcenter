namespace MNX.MonitoringCenter.RigsApi.Core.LifeCycle;

/// <summary>
/// Исключение жизненного цикла.
/// </summary>
public class LifeCycleException : Exception
{
    ///
    public LifeCycleException(string message, string currentStatus)
        : base($"Error: {message}, Current status: {currentStatus}") { }
}
