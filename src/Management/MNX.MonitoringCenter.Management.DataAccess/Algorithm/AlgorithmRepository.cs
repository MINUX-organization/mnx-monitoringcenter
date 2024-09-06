using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Management.UseCases.Algorithm;

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
    public IAsyncEnumerable<string> GetNamesOfAvailableAlgorithms()
    {
        return _context.Algorithms.AsNoTracking()
                                  .Select(x => x.Name)
                                  .AsAsyncEnumerable();
    }

    /// <inheritdoc/>
    public Task<bool> Exists(string name)
    {
        return _context.Algorithms.AnyAsync(x => x.Name.Equals(name));
    }
}