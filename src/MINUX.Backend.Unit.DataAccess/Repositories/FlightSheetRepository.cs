using MINUX.Backend.Unit.Core;
using MINUX.Backend.Unit.UseCases.Abstractions;

namespace MINUX.Backend.Unit.DataAccess.Repositories;

public class FlightSheetRepository : IFlightSheetRepository
{
    private readonly Context _context;

    public FlightSheetRepository(Context context)
    {
        _context = context;
    }

    public Task Add(FlightSheet flightSheet)
    {
        throw new NotImplementedException();
    }

    public Task Apply(Guid flightSheetId)
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<FlightSheet> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task Remove(Guid flightSheetId)
    {
        throw new NotImplementedException();
    }

    public Task Update(FlightSheet flightSheet)
    {
        throw new NotImplementedException();
    }
}