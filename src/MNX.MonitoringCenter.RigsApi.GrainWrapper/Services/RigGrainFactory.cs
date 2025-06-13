using MNX.MonitoringCenter.RigsApi.Core.Services;
using MNX.MonitoringCenter.RigsApi.Core.ValueObjects;

namespace MNX.MonitoringCenter.RigsApi.GrainWrapper.Services;

/// <summary>
/// Реализация <see cref="IRigGrainFactory"/>.
/// </summary>
public class RigGrainFactory : IRigGrainFactory
{
    private readonly IRigRepository _rigRepository;

    public RigGrainFactory(IRigRepository rigRepository)
    {
        _rigRepository = rigRepository ?? throw new ArgumentNullException(nameof(rigRepository));
    }

    /// <inheritdoc/>
    public async Task<IRigGrain?> GetGrain(RigId rigId, CancellationToken cancellationToken = default)
    {
        var rig = await _rigRepository.GetById(rigId, cancellationToken);

        if (rig != null)
        {
            return new RigGrain(rig, _rigRepository);
        }

        return null;
    }
}
