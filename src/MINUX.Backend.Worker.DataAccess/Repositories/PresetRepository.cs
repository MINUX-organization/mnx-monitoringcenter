using Microsoft.EntityFrameworkCore;
using MINUX.Backend.Worker.Core;
using MINUX.Backend.Worker.UseCases.Abstractions;

namespace MINUX.Backend.Worker.DataAccess.Repositories;

public class PresetRepository : IPresetRepository
{
    private readonly Context _context;

    public PresetRepository(Context context)
    {
        _context = context;
    }

    public IAsyncEnumerable<Preset> GetPresets(string? gpuName)
    {
        if (!string.IsNullOrWhiteSpace(gpuName))
        {
            return _context.Presets
                           .Where(x => x.GpuName == gpuName)
                           .AsNoTracking()
                           .AsAsyncEnumerable();
        }

        return _context.Presets.AsNoTracking().AsAsyncEnumerable();
    }

    public async Task<Preset?> GetById(Guid id)
    {
        return await _context.Presets
                             .AsNoTracking()
                             .FirstOrDefaultAsync(x => x.Id == id)
                             .ConfigureAwait(false);
    }

    public async Task Save(Preset preset)
    {
        await _context.Presets.AddAsync(preset).ConfigureAwait(false);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task Update(Preset preset)
    {
        _context.Presets.Update(preset);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task Remove(Preset preset)
    {
        _context.Remove(preset);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }
}