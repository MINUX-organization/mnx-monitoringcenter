using MINUX.Backend.Worker.Core;
using MINUX.Backend.Worker.UseCases.Abstractions;

namespace MINUX.Backend.Worker.DataAccess.Repositories;

public class FlightSheetRepository : IFlightSheetRepository
{
    private readonly DataBaseContext _context;

    public FlightSheetRepository(DataBaseContext context)
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