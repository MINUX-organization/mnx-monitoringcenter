using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Management.UseCases.Miner;

namespace MNX.MonitoringCenter.Management.DataAccess.Miner;

using Miner = Core.Miner.Miner;

/// <summary>
/// Реализация <see cref="IMinerRepository"/>.
/// </summary>
public class MinerRepository : IMinerRepository
{
    private readonly Context _context;

    public MinerRepository(Context context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc/>
    public IAsyncEnumerable<Miner> GetAvailableMiners()
    {
        return _context.Miners
                       .AsNoTracking()
                       .AsAsyncEnumerable();
    }

    /// <inheritdoc/>
    public Task<Miner?> GetMinerById(Guid id)
    {
        return _context.Miners
                       .AsNoTracking()
                       .FirstOrDefaultAsync(x => x.Id == id);
    }

    /// <inheritdoc/>
    public Task<bool> Exists(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.Miners
                       .AsNoTracking()
                       .AnyAsync(x => x.Id == id, cancellationToken);
    }
}