namespace MNX.MonitoringCenter.Management.UseCases.Algorithm;

/// <summary>
/// Репозиторий для доступа к алгоритмам.
/// </summary>
public interface IAlgorithmRepository
{
    /// <summary>
    /// Получить названия доступных алгоритмов.
    /// </summary>
    /// <returns> Список названий алгоритмов. </returns>
    IAsyncEnumerable<string> GetNamesOfAvailableAlgorithms(int userId);

    /// <summary>
    /// Получить список существования алгоритма.
    /// </summary>
    /// <param name="name"> Название алгоритма. </param>
    /// <returns>
    /// <see langword="true"/>, если алгоритм существует, иначе <see langword="false"/>.
    /// </returns>
    Task<bool> Exists(string name);
}
