using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Management.UseCases;
using MNX.MonitoringCenter.Management.Core.Mining.Miner;
using MNX.MonitoringCenter.Management.UseCases.Mining.Miner;

namespace MNX.MonitoringCenter.Management.DataAccess.Miner;

using Miner = Core.Mining.Miner.Miner;

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
    public IAsyncEnumerable<Miner> GetAvailableMiners(Specification specification)
    {
        return _context.Miners
                       .AsNoTrackingWithIdentityResolution()
                       .Include(miner => miner.SupportedAlgorithms)
                       .Filter(specification)
                       .AsAsyncEnumerable();
    }

    /// <inheritdoc/>
    public Task<Miner?> GetMinerById(Guid id, CancellationToken cancellationToken)
    {
        return _context.Miners
                       .AsNoTrackingWithIdentityResolution()
                       .Include(miner => miner.SupportedAlgorithms)
                       .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    /// <inheritdoc/>
    public Task<bool> Exists(Guid id, CancellationToken cancellationToken)
    {
        return _context.Miners
                       .AsNoTracking()
                       .AnyAsync(x => x.Id == id, cancellationToken);
    }

    /// <inheritdoc/>
    public IAsyncEnumerable<MinerAlgorithm> GetMinerAlgorithmsByAlgorithmId(Guid id)
    {
        return _context.MinerAlgorithms
            .Where(x => x.AlgorithmId == id).AsAsyncEnumerable();
    }

    /// <inheritdoc/>
    public async Task AddMinerAlgorithm(MinerAlgorithm minerAlgorithm)
    {
        await _context.MinerAlgorithms.AddAsync(minerAlgorithm);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public Task RemoveAllMinerAlgorithmsById(Guid id)
    {
        return _context.MinerAlgorithms
            .Where(x => x.AlgorithmId == id).ExecuteDeleteAsync();
    }

    /// <inheritdoc/>
    public async Task EditMinerBindingsByAlgorithmId(Guid algorithmId,
                                                     List<string> newNames,
                                                     List<Guid> minerIds)
    {
        var minerAlgorithms = await _context.MinerAlgorithms
            .Where(x => x.AlgorithmId == algorithmId).ExecuteDeleteAsync();

        var algorithmBindingsList = newNames
                .Zip(minerIds, (name, id) => new MinerAlgorithm
                {
                    Name = name,
                    MinerId = id,
                    AlgorithmId = algorithmId
                });

        foreach (var algorithmBinding in algorithmBindingsList)
            await _context.MinerAlgorithms.AddAsync(algorithmBinding);

        await _context.SaveChangesAsync();
    }
}