using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Management.UseCases.Miner;

namespace MNX.MonitoringCenter.Management.DataAccess.Miner;

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
    public IAsyncEnumerable<Core.Miner> GetAvailableMiners()
    {
        return _context.Miners
                       .AsNoTracking()
                       .AsAsyncEnumerable();
    }

    /// <inheritdoc/>
    public Task<bool> Exists(string name, CancellationToken cancellationToken = default)
    {
        return _context.Miners.AsNoTracking().AnyAsync(x => x.Name == name, cancellationToken);
    }
}