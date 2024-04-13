using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Monitoring.Core;
using MNX.MonitoringCenter.Monitoring.UseCases.Abstractions;
using MNX.MonitoringCenter.Monitoring.UseCases.Queries;

namespace MNX.MonitoringCenter.Monitoring.DataAccess.Repositories;

/// <summary>
/// Репозиторий ригов.
/// </summary>
public class RigRepository : IRigRepository
{
    /// <summary>
    /// Контекст БД.
    /// </summary>
    private readonly Context _context;

    public RigRepository(Context context)
    {
        _context = context;
    }

    /// <inheritdoc/>
    public async Task<Rig?> GetById(Guid id, long userId)
    {
        return await _context.Rigs
                             .AsNoTracking()
                             .Where(x => x.UserId == userId)
                             .FirstOrDefaultAsync(x => x.Id == id)
                             .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Rig>> GetAvailable(Specification specification)
    {
        return await _context.Rigs
                             .AsNoTracking()
                             .Available(specification)
                             .Include(x => x.FlightSheetInfo)
                             .ToListAsync()
                             .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Guid>> GetIds(Specification specification)
    {
        return await _context.Rigs
                             .AsNoTracking()
                             .Available(specification)
                             .Filter(specification)
                             .Select(x => x.Id)
                             .ToListAsync()
                             .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<Guid> Add(Rig rig)
    {
        await _context.Rigs.AddAsync(rig).ConfigureAwait(false);
        await _context.SaveChangesAsync().ConfigureAwait(false);

        return rig.Id;
    }

    /// <inheritdoc/>
    public async Task Update(Rig rig)
    {
        _context.Rigs.Update(rig);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task Remove(Rig rig)
    {
        _context.Rigs.Remove(rig);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }
}
