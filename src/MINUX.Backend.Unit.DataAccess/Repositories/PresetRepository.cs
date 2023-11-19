using MINUX.Backend.Unit.Core;
using MINUX.Backend.Unit.UseCases.Abstractions;

namespace MINUX.Backend.Unit.DataAccess.Repositories;

public class PresetRepository : IPresetRepository
{
    private readonly Context _context;

    public PresetRepository(Context context)
    {
        _context = context;
    }

    public Task Add(Preset cryptocurrency)
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<Preset> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task Remove(Guid Id)
    {
        throw new NotImplementedException();
    }

    public Task Upadte(Preset cryptocurrency)
    {
        throw new NotImplementedException();
    }
}