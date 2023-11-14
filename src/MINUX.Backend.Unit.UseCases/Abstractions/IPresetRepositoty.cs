using MINUX.Backend.Unit.Core;

namespace MINUX.Backend.Unit.UseCases.Abstractions;

public interface IPresetRepositoty : IRepository
{
    public IAsyncEnumerable<Preset> GetAll();

    public Task Add(Preset cryptocurrency);

    public Task Upadte(Preset cryptocurrency);

    public Task Remove(Guid Id);
}