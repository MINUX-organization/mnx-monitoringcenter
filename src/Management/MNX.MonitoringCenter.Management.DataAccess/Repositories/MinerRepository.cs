using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;

namespace MNX.MonitoringCenter.Management.DataAccess.Repositories;

/// <summary>
/// Репозиторий для доступа к майнерам
/// </summary>
public class MinerRepository : IMinerRepository
{
    private readonly Context _context;

    public MinerRepository(Context context)
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