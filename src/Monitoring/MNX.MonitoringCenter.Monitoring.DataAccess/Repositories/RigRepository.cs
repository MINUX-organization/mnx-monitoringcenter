using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Monitoring.Core;
using MNX.MonitoringCenter.Monitoring.DataAccess.Dto;
using MNX.MonitoringCenter.Monitoring.UseCases.Abstractions;
using MNX.MonitoringCenter.Monitoring.UseCases.Queries;

namespace MNX.MonitoringCenter.Monitoring.DataAccess.Repositories;

/// <summary>
/// Репозиторий ригов.
/// </summary>
public class RigRepository : IRigRepository
{
    /// <summary>
    /// Контекст БД.
    /// </summary>
    private readonly Context _context;

    /// <summary>
    /// Маппер.
    /// </summary>
    private readonly IMapper _mapper;

    public RigRepository(Context context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(_mapper));
    }

    /// <inheritdoc/>
    public async Task<Rig?> GetById(Guid id, Guid userId)
    {
        var rig = await _context.Rigs
                                .AsNoTracking()
                                .Where(x => x.UserId == userId)
                                .FirstOrDefaultAsync(x => x.Id == id)
                                .ConfigureAwait(false);

        return _mapper.Map<Rig>(rig);
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<Rig> GetList(Specification specification)
    {
        var rigs = _context.Rigs.AsNoTracking()
                            .Available(specification)
                            .Filter(specification)
                            .Include(rig => rig.Devices)
                                .ThenInclude(device => device.FlightSheet)
                                    .ThenInclude(flightSheet => flightSheet!.Coins)
                                        .ThenInclude(coin => coin.Coin)
                            .AsAsyncEnumerable();
        
        await foreach (var rig in rigs)
        {
            yield return _mapper.Map<Rig>(rig);
        }
    }

    /// <inheritdoc/>
    public async Task<RigsSummarizedQuantitativeData> GetRigsSummarizedQuantitativeData(Specification specification)
    {
        var totalData = new RigsSummarizedQuantitativeData();

        await foreach (var rig in _context.Rigs.AsNoTracking().Available(specification)
                                                              .Filter(specification)
                                                              .Include(rig => rig.Devices).AsAsyncEnumerable())
        {
            totalData.TotalRigsCount++;

            totalData.TotalGpusCount.Amd += rig.AmdGpusCount;
            totalData.TotalGpusCount.Nvidia += rig.NvidiaGpusCount;
            totalData.TotalGpusCount.Intel += rig.IntelGpusCount;

            totalData.TotalCpusCount.Intel += rig.IntelCpusCount;
            totalData.TotalCpusCount.Amd += rig.AmdCpusCount;

            totalData.TotalHddsCount += rig.HddsCount;
        }

        return totalData;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Guid>> GetIds(Specification specification)
    {
        return await _context.Rigs
                             .AsNoTracking()
                             .Available(specification)
                             .Filter(specification)
                             .Select(x => x.Id)
                             .ToListAsync()
                             .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<Guid> Add(Rig rig)
    {
        var entity = _mapper.Map<RigDto>(rig);
        await _context.Rigs.AddAsync(entity).ConfigureAwait(false);
        await _context.SaveChangesAsync().ConfigureAwait(false);

        return rig.Id;
    }

    /// <inheritdoc/>
    public async Task Update(Rig rig)
    {
        var entity = _mapper.Map<RigDto>(rig);
        _context.Rigs.Update(entity);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task Remove(Rig rig)
    {
        var entity = _mapper.Map<RigDto>(rig);
        _context.Rigs.Remove(entity);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }
}
