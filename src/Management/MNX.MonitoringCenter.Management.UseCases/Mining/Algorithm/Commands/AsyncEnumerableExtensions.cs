namespace MNX.MonitoringCenter.Management.UseCases.Mining.Algorithm.Commands;

/// <summary>
/// Реализация AnyAsync для перебора асинхронных коллекций.
/// </summary>
public static class AsyncEnumerableExtensions
{
    public static async Task<bool> AnyAsync<T>(this IAsyncEnumerable<T> source, Func<T, bool> predicate)
    {
        await foreach (var item in source)
        {
            if (predicate(item))
            {
                return true;
            }
        }
        return false;
    }
}