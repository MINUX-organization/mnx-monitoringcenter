using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Monitoring.Core.Devices.Gpu;
using MNX.MonitoringCenter.Monitoring.DataAccess.Dto.Devices.Gpu;
using MNX.MonitoringCenter.Monitoring.UseCases.Abstractions;

namespace MNX.MonitoringCenter.Monitoring.DataAccess.Repositories.MiningDevices;

/// <summary>
/// Реализация <see cref="IMiningDeviceRepository"/> и <see cref="IGpuRepository"/>.
/// </summary>
public class GpuRepository : MiningDeviceRepository, IGpuRepository
{
    public GpuRepository(Context context, IMapper mapper) :  base(context, mapper) { }

    /// <inheritdoc/>
    public async Task<GpuOverclocking?> GetOverclocking(Guid id)
    {
        var overclocking = await _context.Overclocking
                                         .AsNoTracking()
                                         .FirstOrDefaultAsync(x => x.MiningDeviceId == id)
                                         .ConfigureAwait(false);

        return overclocking is null ? _mapper.Map<GpuOverclocking>(overclocking) : null;
    }

    /// <inheritdoc/>
    public async Task SetOverclocking(Guid id, GpuOverclocking overclocking)
    {
        var gpu = await _context.MiningDevices.AsNoTracking()
                                              .OfType<GpuDto>()
                                              .FirstAsync(x => x.Id == id)
                                              .ConfigureAwait(false);

        GpuOverclockingDto overclockingDto;

        if (gpu.OverclockingId is null)
        {
            overclockingDto = _mapper.Map<GpuOverclockingDto>(overclocking);
            overclockingDto.MiningDeviceId = id;
            await _context.Overclocking.AddAsync(overclockingDto);

            gpu.OverclockingId = overclockingDto.Id;
            _context.MiningDevices.Update(gpu);
        }
        else
        {
            overclockingDto = await _context.Overclocking
                                            .FirstAsync(x => x.Id == gpu.OverclockingId)
                                            .ConfigureAwait(false);

            overclockingDto.MiningDeviceId = id;

            overclockingDto.CoreClockOffset = overclocking.CoreClockOffset;
            overclockingDto.CoreClockLock = overclocking.CoreClockLock;
            overclockingDto.CoreVoltage = overclocking.CoreVoltage;
            overclockingDto.CoreVoltageOffset = overclocking.CoreVoltageOffset;

            overclockingDto.MemoryClockOffset = overclocking.MemoryClockOffset;
            overclockingDto.MemoryClockLock = overclocking.MemoryClockLock;
            overclockingDto.MemoryVoltage = overclocking.MemoryVoltage;
            overclockingDto.MemoryVoltageOffset = overclocking.MemoryVoltageOffset;

            overclockingDto.CriticalTemperature = overclocking.CriticalTemperature;
            overclockingDto.FanSpeed = overclocking.FanSpeed;
            overclockingDto.PowerLimit = overclocking.PowerLimit;
        }

        await _context.SaveChangesAsync();
    }
}
