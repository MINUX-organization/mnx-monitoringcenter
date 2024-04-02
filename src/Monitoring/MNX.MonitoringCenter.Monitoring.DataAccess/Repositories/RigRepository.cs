using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Monitoring.Core;
using MNX.MonitoringCenter.Monitoring.UseCases.Abstractions;

namespace MNX.MonitoringCenter.Monitoring.DataAccess.Repositories;

/// <summary>
/// Репозиторий ригов.
/// </summary>
public class RigRepository : IRigRepository
{
    private readonly Context _context;

    public RigRepository(Context context)
    {
        _context = context;
    }

    public async Task<Rig?> GetById(Guid id, long userId)
    {
        return await _context.Rigs
                             .AsNoTracking()
                             .Where(x => x.UserId == userId)
                             .FirstOrDefaultAsync(x => x.Id == id)
                             .ConfigureAwait(false);
    }

    public IAsyncEnumerable<Rig> GetAvailable(long userId)
    {
        return _context.Rigs
                       .Where(x => x.UserId == userId)
                       .Include(x => x.FlightSheetInfo)
                       .AsNoTracking()
                       .AsAsyncEnumerable();
    }

    public async Task<Guid> Add(Rig rig)
    {
        await _context.Rigs.AddAsync(rig).ConfigureAwait(false);
        await _context.SaveChangesAsync().ConfigureAwait(false);

        return rig.Id;
    }

    public async Task Update(Rig rig)
    {
        _context.Rigs.Update(rig);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task Remove(Rig rig)
    {
        _context.Rigs.Remove(rig);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }
}
