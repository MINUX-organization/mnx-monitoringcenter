using System.Collections.Concurrent;

namespace MNX.MonitoringCenter.Monitoring.UseCases;

/// <summary>
/// Класс для подсчета количества подключений.
/// </summary>
public class ConnectionCounter
{
    /// <summary>
    /// Словарь подключений.
    /// </summary>
    private readonly ConcurrentDictionary<long, int> _groupConnections = new();

    /// <summary>
    /// Добавить соединение в группу
    /// </summary>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <returns> Количество подключений. </returns>
    public int AddConnectionInGroup(long userId)
    {
        return _groupConnections.AddOrUpdate(userId, 1, (_, value) => Interlocked.Increment(ref value));
    }

    /// <summary>
    /// Добавить подключение из группы.
    /// </summary>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <returns> Количество оставшихся подключений. </returns>
    public int RemoveConnectionInGroup(long userId)
    {
        if (_groupConnections.TryGetValue(userId, out int connectionCount))
        {
            if (Interlocked.Decrement(ref connectionCount) <= 0)
            {
                _groupConnections.TryRemove(userId, out _);
            }

            return connectionCount;
        }

        return 0;
    }
}
