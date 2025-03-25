using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Management.UseCases.Mining.Algorithm;
using MNX.MonitoringCenter.Management.Contracts.AlgorithmBinding;

namespace MNX.MonitoringCenter.Management.DataAccess.MinerAlgorithm;

using MinerAlgorithm = Core.Mining.Miner.MinerAlgorithm;

public class MinerAlgorithmRepository : IMinerAlgorithmRepository
{
    private readonly Context _context;

    public MinerAlgorithmRepository(Context context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc/>
    public IAsyncEnumerable<MinerAlgorithm> GetMinerAlgorithmsByAlgorithmId(Guid id)
    {
        return _context.MinerAlgorithms.AsNoTracking()
            .Where(x => x.AlgorithmId == id).AsAsyncEnumerable();
    }

    /// <inheritdoc/>
    public async Task AddRangeAsync(List<MinerAlgorithm> minerAlgorithms)
    {
        await _context.MinerAlgorithms.AddRangeAsync(minerAlgorithms);
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
                                                     List<RelativeNameBindingModel> bindings)
    {
        var minerAlgorithms = await _context.MinerAlgorithms
            .AsNoTracking()
            .Where(x => x.AlgorithmId == algorithmId)
            .ToListAsync();

        _context.MinerAlgorithms.RemoveRange(minerAlgorithms);

        await _context.MinerAlgorithms.AddRangeAsync(bindings.Select(x => new MinerAlgorithm()
        {
            Name = x.RelativeName,
            MinerId = x.MinerId,
            AlgorithmId = algorithmId
        }));

        await _context.SaveChangesAsync();
    }
}
