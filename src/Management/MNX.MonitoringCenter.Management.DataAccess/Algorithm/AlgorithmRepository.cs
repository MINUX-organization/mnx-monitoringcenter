using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Management.UseCases;
using MNX.MonitoringCenter.Management.UseCases.Mining.Algorithm;

namespace MNX.MonitoringCenter.Management.DataAccess.Algorithm;

using Algorithm = Core.Mining.Algorithm;

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
    public IAsyncEnumerable<Algorithm> GetAvailable(Specification specification)
    {
        return _context.Algorithms
            .AsNoTracking()
            .Available(specification)
            .OrderBy(e => e.OwnerId == null)
            .ThenBy(e => e.OwnerId)
            .AsAsyncEnumerable();
    }

    /// <inheritdoc/>
    public Task<Algorithm?> GetById(Guid id,
                                    Guid userId,
                                    CancellationToken cancellationToken = default)
    {
        return _context.Algorithms.Available(userId)
            .AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    /// <inheritdoc/>
    public Task<bool> Exists(Guid userId,
                             string name,
                             CancellationToken cancellationToken)
    {
        return _context.Algorithms.AsNoTracking().Available(userId)
                .AnyAsync(x => x.Name == name, cancellationToken);
    }

    /// <inheritdoc/>
    public Task<bool> Exists(Guid algorithmId,
                             string name,
                                   Guid userId,
                             CancellationToken cancellationToken)
    {
        return _context.Algorithms.AsNoTracking()
            .Available(userId)
            .Where(x => x.Id != algorithmId)
            .AnyAsync(x => x.Name == name, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task AddAsync(Algorithm algorithm)
    {
        await _context.Algorithms.AddAsync(algorithm);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public Task Remove(Guid id, Guid userId)
    {
        return _context.Algorithms
            .Where(x => x.Id == id && x.OwnerId == userId)
            .ExecuteDeleteAsync();
    }

    /// <inheritdoc/>
    public Task EditAlgorithmName(Guid algorithmId,
                                  Guid userId,
                                  string newName)
    {
        return _context.Algorithms
            .Where(x => x.Id == algorithmId && x.OwnerId == userId)
                .ExecuteUpdateAsync(x => x.SetProperty(a => a.Name, newName));
    }
}