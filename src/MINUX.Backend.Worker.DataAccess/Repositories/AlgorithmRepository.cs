using Microsoft.EntityFrameworkCore;
using MINUX.Backend.Worker.UseCases.Abstractions;

namespace MINUX.Backend.Worker.DataAccess.Repositories;

public class AlgorithmRepository : IAlgorithmRepository
{
    private readonly DataBaseContext _context;

    public AlgorithmRepository(DataBaseContext context)
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