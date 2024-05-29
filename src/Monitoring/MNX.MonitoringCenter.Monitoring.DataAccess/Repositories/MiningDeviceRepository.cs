using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Monitoring.Core;
using MNX.MonitoringCenter.Monitoring.Core.Devices;
using MNX.MonitoringCenter.Monitoring.DataAccess.Dto;
using MNX.MonitoringCenter.Monitoring.DataAccess.Dto.Devices;
using MNX.MonitoringCenter.Monitoring.UseCases.Abstractions;
using MNX.MonitoringCenter.Monitoring.UseCases.Queries.Devices;

namespace MNX.MonitoringCenter.Monitoring.DataAccess.Repositories;

/// <summary>
/// Реализация <see cref="IMiningDeviceRepository"/>.
/// </summary>
public class MiningDeviceRepository : IMiningDeviceRepository
{
    /// <summary>
    /// Контекст БД.
    /// </summary>
    private readonly Context _context;

    /// <summary>
    /// Автомаппер.
    /// </summary>
    private readonly IMapper _mapper;

    public MiningDeviceRepository(Context context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(_mapper));
    }

    /// <inheritdoc/>
    public async Task<Overclocking?> GetOverclockingById(Guid id)
    {
        var device = await _context.MiningDevices
            .Where(d => d.Id == id)
            .Include(d => d.Overclocking)
            .Include(device => device.Rig)
            .Include(device => device.FlightSheet)
            .SingleOrDefaultAsync();

        if (device is CpuDto cpuDto)
        {
            throw new NotImplementedException();
        }
        else if (device is GpuDto gpuDto)
        {
            return _mapper.Map<Overclocking>(device.Overclocking);
        }
        else if (device is HddDto hddDto)
        {
            throw new NotImplementedException();
        }
        else
        {
            throw new NotImplementedException();
        }
    }

    public async Task SetOverclockingById(Guid id, Overclocking overclocking)
    {
        var overclockingDto = _mapper.Map<OverclockingDto>(overclocking);
        await _context.Overclocking.AddAsync(overclockingDto);
        await _context.SaveChangesAsync();

        var device = await _context.MiningDevices.Where(d => d.Id == id).SingleOrDefaultAsync();
        if (device == null) 
        {
            return;
        }

        device.OverclockingId = overclockingDto.Id;
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<MiningDevice> GetList(DeviceSpecification specification)
    {
        var devices = _context.MiningDevices
                              .Available(specification)
                              .Filter(specification)
                              .Include(device => device.Rig)
                              .Include(device => device.FlightSheet)
                              .AsAsyncEnumerable();

        await foreach (var device in devices)
        {
            if (device is CpuDto cpuDto)
            {
                yield return _mapper.Map<Cpu>(cpuDto);
            }
            else if (device is GpuDto gpuDto)
            {
                yield return _mapper.Map<Gpu>(gpuDto);
            }
            else if (device is HddDto hddDto)
            {
                yield return _mapper.Map<Hdd>(hddDto);
            }
        }
    }
}
