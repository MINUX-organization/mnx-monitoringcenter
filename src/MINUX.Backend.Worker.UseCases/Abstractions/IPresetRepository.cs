using MINUX.Backend.Worker.Core;

namespace MINUX.Backend.Worker.UseCases.Abstractions;

public interface IPresetRepository
{
    public IAsyncEnumerable<Preset> GetPresets(string? gpuName);

    public Task Save(Preset preset);

    public Task Update(Preset preset);

    public Task Remove(Preset preset);
}