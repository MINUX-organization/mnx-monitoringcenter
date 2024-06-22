using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Monitoring.Core;
using MNX.MonitoringCenter.Monitoring.DataAccess.Dto;
using MNX.MonitoringCenter.Monitoring.UseCases.Abstractions;

namespace MNX.MonitoringCenter.Monitoring.DataAccess.Repositories;

/// <summary>
/// Реализация <see cref="IOverclockingRepository"/>.
/// </summary>
public class OverclockingRepository : IOverclockingRepository
{
    /// <summary>
    /// Контекст БД.
    /// </summary>
    private readonly Context _context;

    /// <summary>
    /// Автомаппер.
    /// </summary>
    private readonly IMapper _mapper;

    public OverclockingRepository(Context context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(_mapper));
    }

    public async Task<Overclocking?> GetOverclockingById(Guid id)
    {
        var device = await _context.MiningDevices
            .Where(d => d.Id == id)
            .Include(d => d.Overclocking)
            .SingleOrDefaultAsync();

        return device != null && device.Overclocking != null
            ? _mapper.Map<Overclocking>(device.Overclocking)
            : null;
    }

    public async Task SetOverclockingById(Guid id, Overclocking overclocking)
    {
        var overclockingDto = _mapper.Map<OverclockingDto>(overclocking);
        await _context.Overclocking.AddAsync(overclockingDto);
        await _context.SaveChangesAsync();

        var device = await _context.MiningDevices.Where(d => d.Id == id).SingleOrDefaultAsync();
        if (device == null)
            return;

        device.OverclockingId = overclockingDto.Id;
        await _context.SaveChangesAsync();
    }
}
