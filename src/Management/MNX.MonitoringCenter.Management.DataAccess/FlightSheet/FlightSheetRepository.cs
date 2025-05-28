using AutoMapper;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using MNX.MonitoringCenter.Management.UseCases;
using MNX.MonitoringCenter.Management.DataAccess.FlightSheet.Dto;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet;

namespace MNX.MonitoringCenter.Management.DataAccess.FlightSheet;

using FlightSheet = Core.Mining.FlightSheet.FlightSheet;

/// <summary>
/// Реализация <see cref="IFlightSheetRepository"/>.
/// </summary>
public class FlightSheetRepository : IFlightSheetRepository
{
    private readonly Context _context;

    private readonly IMapper _mapper;

    public FlightSheetRepository(Context context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <inheritdoc/>
    public IAsyncEnumerable<FlightSheet> GetAllAvailable(Specification specification)
    {
        return GetFlightSheets(specification.UserId)
            .Filter(specification)
            .ProjectTo<FlightSheet>(_mapper.ConfigurationProvider)
            .AsAsyncEnumerable();
    }

    /// <inheritdoc/>
    public Task<FlightSheet?> GetAvailableById(Guid flightSheetId, Guid userId,
                                               CancellationToken cancellationToken)
    {
        return GetFlightSheets(userId)
            .ProjectTo<FlightSheet>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(x => x.Id == flightSheetId, cancellationToken);
    }

    /// <inheritdoc/>
    public Task<bool> ExistsAvailable(string name, Guid userId, CancellationToken cancellationToken)
    {
        return _context.FlightSheets.AsNoTracking()
                                    .Where(x => x.UserId == userId)
                                    .AnyAsync(x => x.Name == name, cancellationToken);
    }

    /// <inheritdoc/>
    public Task<bool> Exists(Guid id, CancellationToken cancellationToken)
    {
        return _context.FlightSheets.AsNoTracking()
                                    .AnyAsync(x => x.Id == id, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task Add(FlightSheet flightSheet)
    {
        var dto = _mapper.Map<FlightSheetDto>(flightSheet);
        await _context.FlightSheets.AddAsync(dto);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task Edit(FlightSheet flightSheet)
    {
        var dto = _mapper.Map<FlightSheetDto>(flightSheet);

        var oldFlightSheet = await _context.FlightSheets
                                        .Where(x => x.UserId == dto.UserId)
                                        .Include(x => x.Targets)
                                        .FirstAsync(x => x.Id == dto.Id);

        _context.FlightSheetTargets.RemoveRange(oldFlightSheet.Targets);
        oldFlightSheet.Name = dto.Name;
        await _context.FlightSheetTargets.AddRangeAsync(dto.Targets);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public Task Remove(Guid id, Guid userId)
    {
        return _context.FlightSheets
            .Where(x => x.Id == id && x.UserId == userId)
            .ExecuteDeleteAsync();
    }

    /// <summary>
    /// Получить запрашиваемый список полётных листов.
    /// </summary>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <returns> Запрашиваемый список полётных листов. </returns>
    private IQueryable<FlightSheetDto> GetFlightSheets(Guid userId)
    {
        return _context.FlightSheets.AsNoTrackingWithIdentityResolution()
                                    .AsSplitQuery()
                                    .Where(x => x.UserId == userId)
                                    .Include(x => x.Targets)
                                        .ThenInclude(target => target.Miner)
                                            .ThenInclude(miner => miner!.SupportedAlgorithms)
                                    .Include(x => x.Targets)
                                        .ThenInclude(target => target.CoinConfigs)
                                            .ThenInclude(config => config.Pool)
                                                .ThenInclude(pool => pool!.Cryptocurrency)
                                                    .ThenInclude(cryptocurrency => cryptocurrency!.Algorithm)
                                    .Include(x => x.Targets)
                                        .ThenInclude(target => target.CoinConfigs)
                                            .ThenInclude(config => config.Wallet)
                                                .ThenInclude(wallet => wallet!.Cryptocurrency)
                                                    .ThenInclude(cryptocurrency => cryptocurrency!.Algorithm);
    }
}