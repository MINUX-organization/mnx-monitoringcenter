using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.RigsApi.Core;
using MNX.MonitoringCenter.RigsApi.Core.Services;
using MNX.MonitoringCenter.RigsApi.Core.ValueObjects;

namespace MNX.MonitoringCenter.RigsApi.DataAccess;

/// <summary>
/// Реализация <see cref="IRigRepository"/>.
/// </summary>
public class RigRepository : IRigRepository
{
    private readonly Context _context;

    public RigRepository(Context context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc/>
    public IAsyncEnumerable<Rig> GetAvailable(Guid userId)
    {
        return _context.Rigs.AsNoTracking()
                            .Where(rig => rig.OwnerId == userId)
                            .Select(dto => new Rig(
                                new RigId(dto.Id),
                                dto.OwnerId,
                                dto.Name,
                                dto.CurrentInventoryId,
                                dto.IsOnline,
                                dto.LifeCycleStatus,
                                dto.MiningLifeCycleStatus
                             ))
                            .AsAsyncEnumerable();
    }

    /// <inheritdoc/>
    public async Task<Rig?> GetById(RigId id, CancellationToken cancellationToken = default)
    {
        var dto = await _context.Rigs.AsNoTracking()
            .FirstOrDefaultAsync(rig => rig.Id == id, cancellationToken);

        if (dto is null)
            return null;

        return new Rig(new RigId(dto.Id),
                       dto.OwnerId,
                       dto.Name,
                       dto.CurrentInventoryId,
                       dto.IsOnline,
                       dto.LifeCycleStatus,
                       dto.MiningLifeCycleStatus
        );
    }

    /// <inheritdoc/>
    public Task Add(Rig rig, CancellationToken cancellationToken = default)
    {
        var dto = new RigDto()
        {
            Id = rig.Id,
            OwnerId = rig.OwnerId,
            Name = rig.Name,
            CurrentInventoryId = rig.CurrentInventoryId,
            IsOnline = rig.IsOnline,
            LifeCycleStatus = rig.LifeCycleStatus,
            MiningLifeCycleStatus = rig.MiningLifeCycleStatus
        };

        _context.Rigs.Add(dto);
        return _context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public Task Update(Rig rig, CancellationToken cancellationToken = default)
    {
        var dto = new RigDto()
        {
            Id = rig.Id,
            OwnerId = rig.OwnerId,
            Name = rig.Name,
            CurrentInventoryId = rig.CurrentInventoryId,
            IsOnline = rig.IsOnline,
            LifeCycleStatus = rig.LifeCycleStatus,
            MiningLifeCycleStatus = rig.MiningLifeCycleStatus
        };

        _context.Update(dto);
        return _context.SaveChangesAsync(cancellationToken);
    }
}
