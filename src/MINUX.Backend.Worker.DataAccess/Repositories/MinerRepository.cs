using Microsoft.EntityFrameworkCore;
using MINUX.Backend.Worker.Core;
using MINUX.Backend.Worker.UseCases.Abstractions;

namespace MINUX.Backend.Worker.DataAccess.Repositories;

/// <summary>
/// Репозиторий для доступа к майнерам
/// </summary>
public class MinerRepository : IMinerRepository
{
    private readonly DataBaseContext _context;

    public MinerRepository(DataBaseContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <summary>
    /// Получить список доступных майнеров
    /// </summary>
    /// <returns></returns>
    public IAsyncEnumerable<Miner> GetAvailableMiners()
    {
        return _context.Miners
                       .AsNoTracking()
                       .Include(miner => miner.Algorithms)
                       .Select(x => new Miner() { Name = x.Name, Algorithms = x.Algorithms.Select(x => new Algorithm() { Name = x.AlgorithmName }).ToList() })
                       .AsAsyncEnumerable();
    }
}