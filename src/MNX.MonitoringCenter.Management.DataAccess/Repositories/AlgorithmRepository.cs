using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;

namespace MNX.MonitoringCenter.Management.DataAccess.Repositories;

public class AlgorithmRepository : IAlgorithmRepository
{
    private readonly Context _context;

    public AlgorithmRepository(Context context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public IAsyncEnumerable<string> GetNamesOfAvailableAlgorithms()
    {
        return _context.Algorithms.AsNoTracking().Select(x => x.Name).AsAsyncEnumerable();
    }

    public async Task<bool> Exists(string name)
    {
        return await _context.Algorithms
                             .AnyAsync(x => x.Name.Equals(name))
                             .ConfigureAwait(false);
    }
}