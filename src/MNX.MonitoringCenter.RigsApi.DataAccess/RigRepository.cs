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
        return _context.Rigs
            .AsNoTracking()
            .Where(rig => rig.OwnerId == userId)
            .Where(rig => !rig.IsDecommissioned)
            .Select(dto => dto.ToDomain())
            .AsAsyncEnumerable();
    }

    /// <inheritdoc/>
    public async Task<Rig?> GetById(RigId id, CancellationToken cancellationToken = default)
    {
        var dto = await _context.Rigs
            .AsNoTracking()
            .FirstOrDefaultAsync(rig => rig.Id == id, cancellationToken);

        return dto?.ToDomain();
    }

    /// <inheritdoc/>
    public Task Add(Rig rig, CancellationToken cancellationToken = default)
    {
        _context.Rigs.Add(RigDto.FromDomain(rig));
        return _context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public Task Update(Rig rig, CancellationToken cancellationToken = default)
    {
        _context.Update(RigDto.FromDomain(rig));
        return _context.SaveChangesAsync(cancellationToken);
    }
}
