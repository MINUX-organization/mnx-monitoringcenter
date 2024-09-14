using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Management.Core.FlightSheet;

namespace MNX.MonitoringCenter.Management.DataAccess.FlightSheet;

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
    public IAsyncEnumerable<FlightSheetBase> GetAllAvailable(Guid userId)
    {
        return GetFlightSheets(userId).AsAsyncEnumerable();
    }

    /// <inheritdoc/>
    public Task<FlightSheetBase?> GetAvailableById(Guid flightSheetId, Guid userId, CancellationToken cancellationToken)
    {
        return GetFlightSheets(userId).FirstOrDefaultAsync(x => x.Id == flightSheetId, cancellationToken);
    }

    /// <inheritdoc/>
    public Task<bool> Exists(string name, Guid userId, CancellationToken cancellationToken)
    {
        return _context.FlightSheets.AsNoTracking()
                                    .Where(x => x.UserId == userId)
                                    .AnyAsync(x => x.Name == name, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task Add(FlightSheetBase flightSheet, CancellationToken cancellationToken)
    {
        await _context.FlightSheets.AddAsync(flightSheet, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task Edit(FlightSheetBase flightSheet, CancellationToken cancellationToken)
    {
        var oldFlightSheet = await GetFlightSheets(flightSheet.UserId).FirstAsync(x => x.Id == flightSheet.Id, cancellationToken);

        _context.FlightSheetConfigs.RemoveRange(oldFlightSheet.Configs);

        _context.FlightSheets.Update(flightSheet);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public Task Remove(FlightSheetBase flightSheet, CancellationToken cancellationToken)
    {
        _context.FlightSheetConfigs.RemoveRange(flightSheet.Configs);
        _context.FlightSheets.Remove(flightSheet);
        return _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Получить запрашиваемый список полётных листов.
    /// </summary>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <returns> Запрашиваемый список полётных листов. </returns>
    private IQueryable<FlightSheetBase> GetFlightSheets(Guid userId)
    {
        return _context.FlightSheets.AsNoTrackingWithIdentityResolution()
                                    .Where(x => x.UserId == userId)
                                    .Include(x => x.Miner)
                                    .Include(x => x.Configs)
                                        .ThenInclude(config => config.Pool)
                                            .ThenInclude(pool => pool!.Cryptocurrency)
                                    .Include(x => x.Configs)
                                        .ThenInclude(config => config.Wallet)
                                            .ThenInclude(wallet => wallet!.Cryptocurrency);
    }
}