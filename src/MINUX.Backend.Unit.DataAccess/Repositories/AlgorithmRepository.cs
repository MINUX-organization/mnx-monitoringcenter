using Microsoft.EntityFrameworkCore;
using MINUX.Backend.Unit.Core;
using MINUX.Backend.Unit.UseCases.Abstractions;

namespace MINUX.Backend.Unit.DataAccess.Repositories;

public class AlgorithmRepository : IAlgorithmRepository
{
    private readonly Context _context;

    public AlgorithmRepository(Context context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public IAsyncEnumerable<Algorithm> GetAll()
    {
        return _context.Algorithms.AsNoTracking().AsAsyncEnumerable();
    }

    public async Task<bool> Exists(string name)
    {
        return ! (await _context.Algorithms.AsNoTracking().FirstOrDefaultAsync(x => x.Name == name) == null);
    }
}