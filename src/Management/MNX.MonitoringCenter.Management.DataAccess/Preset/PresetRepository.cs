using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Management.UseCases.Presets;

namespace MNX.MonitoringCenter.Management.DataAccess.Preset;

using Preset = Core.Preset;

/// <summary>
/// Реализация <see cref="IPresetRepository"/>.
/// </summary>
public class PresetRepository : IPresetRepository
{
    private readonly Context _context;

    public PresetRepository(Context context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc/>
    public IAsyncEnumerable<Preset> GetAllAvailable(string? gpuName, Guid userId)
    {
        if (!string.IsNullOrWhiteSpace(gpuName))
        {
            return _context.Presets
                           .Include(x => x.Overclocking)
                           .Where(x => x.GpuName == gpuName)
                           .Where(x => x.UserId == userId)
                           .AsNoTracking()
                           .AsAsyncEnumerable();
        }

        return _context.Presets
                       .Include(x => x.Overclocking)
                       .Where(x => x.UserId == userId)
                       .AsNoTracking()
                       .AsAsyncEnumerable();
    }

    /// <inheritdoc/>
    public Task<Preset?> GetAvailableById(Guid id, Guid userId)
    {
        return _context.Presets.AsNoTracking()
                               .Where(x => x.UserId == userId)
                               .FirstOrDefaultAsync(x => x.Id == id);
    }

    /// <inheritdoc/>
    public Task<bool> Exists(Guid userId, string name)
    {
        return _context.Presets.AsNoTracking()
                               .Where(x => x.UserId == userId)
                               .AnyAsync(x => x.Name.Equals(name));
    }

    /// <inheritdoc/>
    public async Task Save(Preset preset)
    {
        await _context.Presets.AddAsync(preset);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public Task Update(Preset preset)
    {
        _context.Presets.Update(preset);
        return _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public Task Remove(Preset preset)
    {
        _context.Presets.Remove(preset);
        return _context.SaveChangesAsync();
    }
}