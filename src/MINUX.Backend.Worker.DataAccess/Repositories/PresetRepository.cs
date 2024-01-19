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
            return _context.Presets.Where(x => x.GpuName == gpuName).AsAsyncEnumerable();
        }

        return _context.Presets.AsAsyncEnumerable();
    }

    public async Task Save(Preset preset)
    {
        await _context.Presets.AddAsync(preset);
        await _context.SaveChangesAsync();
    }

    public Task Update(Preset preset)
    {
        throw new NotImplementedException();
    }

    public async Task Remove(Preset preset)
    {
        _context.Remove(preset);
        await _context.SaveChangesAsync();
    }
}