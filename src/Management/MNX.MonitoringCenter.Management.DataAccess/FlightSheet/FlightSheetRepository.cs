using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Management.UseCases.FlightSheet;

namespace MNX.MonitoringCenter.Management.DataAccess.FlightSheet;

using FlightSheet = Core.FlightSheet.FlightSheet;

/// <summary>
/// Реализация <see cref="IFlightSheetRepository"/>.
/// </summary>
public class FlightSheetRepository : IFlightSheetRepository
{
    private readonly Context _context;

    public FlightSheetRepository(Context context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc/>
    public IAsyncEnumerable<FlightSheet> GetAllAvailable(Guid userId)
    {
        return GetFlightSheets(userId).AsAsyncEnumerable();
    }

    /// <inheritdoc/>
    public Task<FlightSheet?> GetAvailableById(Guid flightSheetId, Guid userId,
                                               CancellationToken cancellationToken)
    {
        return GetFlightSheets(userId)
            .FirstOrDefaultAsync(x => x.Id == flightSheetId, cancellationToken);
    }

    /// <inheritdoc/>
    public Task<bool> Exists(string name, Guid userId, CancellationToken cancellationToken)
    {
        return _context.FlightSheets.AsNoTracking()
                                    .Where(x => x.UserId == userId)
                                    .AnyAsync(x => x.Name == name, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task Add(FlightSheet flightSheet, CancellationToken cancellationToken)
    {
        await _context.FlightSheets.AddAsync(flightSheet, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task Edit(FlightSheet flightSheet, CancellationToken cancellationToken)
    {
        var oldFlightSheet = await _context.FlightSheets
                                        .Where(x => x.UserId == flightSheet.UserId)
                                        .Include(x => x.Targets)
                                        .FirstAsync(x => x.Id == flightSheet.Id, cancellationToken);

        _context.FlightSheetTargets.RemoveRange(oldFlightSheet.Targets);
        oldFlightSheet.Name = flightSheet.Name;
        await _context.FlightSheetTargets.AddRangeAsync(flightSheet.Targets, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public Task Remove(Guid id, Guid userId, CancellationToken cancellationToken)
    {
        return _context.FlightSheets
            .Where(x => x.Id == id && x.UserId == userId)
            .ExecuteDeleteAsync(cancellationToken);
    }

    /// <summary>
    /// Получить запрашиваемый список полётных листов.
    /// </summary>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <returns> Запрашиваемый список полётных листов. </returns>
    private IQueryable<FlightSheet> GetFlightSheets(Guid userId)
    {
        return _context.FlightSheets.AsNoTrackingWithIdentityResolution()
                                    .Where(x => x.UserId == userId)
                                    .Include(x => x.Targets)
                                        .ThenInclude(target => target.Miner)
                                            .ThenInclude(miner => miner!.DeviceTypes)
                                    .Include(x => x.Targets)
                                        .ThenInclude(target => target.Configs)
                                            .ThenInclude(config => config.Pool)
                                                .ThenInclude(pool => pool!.Cryptocurrency)
                                    .Include(x => x.Targets)
                                        .ThenInclude(target => target.Configs)
                                            .ThenInclude(config => config.Wallet)
                                                .ThenInclude(wallet => wallet!.Cryptocurrency);
    }
}