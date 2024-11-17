using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Management.UseCases.Presets;
using System.Linq.Expressions;
using MNX.MonitoringCenter.Management.UseCases;

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
    public IAsyncEnumerable<Preset> GetAllAvailable(string? gpuName, Specification specification)
    {
        var presets = _context.Presets.AsNoTrackingWithIdentityResolution()
                                                      .Include(x => x.Overclocking)
                                                      .Where(x => x.UserId == specification.UserId)
                                                      .Filter(specification);

        return string.IsNullOrWhiteSpace(gpuName) 
            ? presets.AsAsyncEnumerable()
            : presets.Where(x => x.GpuName == gpuName).AsAsyncEnumerable();
    }

    /// <inheritdoc/>
    public Task<Dictionary<string, List<Preset>>> GetGroupedList(
            Expression<Func<Preset, string>> expression, Specification specification, CancellationToken cancellationToken)
    {
        return _context.Presets.AsNoTrackingWithIdentityResolution()
                               .Where(x => x.UserId == specification.UserId)
                               .Filter(specification)
                               .Include(x => x.Overclocking)
                               .GroupBy(expression)
                               .ToDictionaryAsync(g => g.Key, g => g.ToList(), cancellationToken);
    }

    /// <inheritdoc/>
    public Task<Preset?> GetAvailableById(Guid id, Guid userId, CancellationToken cancellationToken)
    {
        return _context.Presets.AsNoTracking()
                               .Where(x => x.UserId == userId)
                               .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    /// <inheritdoc/>
    public Task<bool> Exists(Guid userId, string name, CancellationToken cancellationToken)
    {
        return _context.Presets.AsNoTracking()
                               .Where(x => x.UserId == userId)
                               .AnyAsync(x => x.Name.Equals(name), cancellationToken);
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
    public Task Remove(Guid id, Guid userId)
    {
        return _context.Presets
            .Where(x => x.Id == id && x.UserId == userId)
            .ExecuteDeleteAsync();
    }
}