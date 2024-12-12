using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Management.UseCases;
using MNX.MonitoringCenter.Management.UseCases.Mining.Algorithm;

namespace MNX.MonitoringCenter.Management.DataAccess.Algorithm;

/// <summary>
/// Реализация <see cref="IAlgorithmRepository"/>.
/// </summary>
public class AlgorithmRepository : IAlgorithmRepository
{
    private readonly Context _context;

    public AlgorithmRepository(Context context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc/>
    public IAsyncEnumerable<Core.Mining.Algorithm> GetNamesOfAvailableAlgorithms(Specification specification)
    {
        return _context.Algorithms.Filter(specification)
                                  .AsNoTracking()
                                  .AsAsyncEnumerable();
    }

    /// <inheritdoc/>
    public Task<Core.Mining.Algorithm?> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.Algorithms
                       .AsNoTracking()
                       .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}