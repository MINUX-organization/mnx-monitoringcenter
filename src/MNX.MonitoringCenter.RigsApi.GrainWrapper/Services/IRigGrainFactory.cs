using MNX.MonitoringCenter.RigsApi.Core.ValueObjects;

namespace MNX.MonitoringCenter.RigsApi.GrainWrapper.Services;

/// <summary>
/// Фабрика <see cref="IRigGrain"/>.
/// </summary>
public interface IRigGrainFactory
{
    /// <summary>
    /// Получить зерно рига.
    /// </summary>
    /// <param name="rigId"> Идентификатор рига. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns> Зерно рига. </returns>
    Task<IRigGrain?> GetGrain(RigId rigId, CancellationToken cancellationToken = default);
}
