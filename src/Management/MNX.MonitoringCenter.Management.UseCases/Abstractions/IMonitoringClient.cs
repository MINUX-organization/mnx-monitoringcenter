using Refit;

namespace MNX.MonitoringCenter.Management.UseCases.Abstractions;

/// <summary>
/// Http клиент для сервиса мониторинга
/// </summary>
public interface IMonitoringClient
{
    /// <summary>
    /// Получить признак наличия GPU с переданным именем.
    /// </summary>
    /// <param name="gpuName"> Имя видеокарты </param>
    /// <returns> <see langword="true"/>, если GPU существует, иначе <see langword="false"/> </returns>
    [Get("/")]
    public Task<bool> GpuExists(long userId, [Query] string gpuName);
}
