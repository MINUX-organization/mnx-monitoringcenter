using MNX.MonitoringCenter.Management.UseCases.FlightSheet;

namespace MNX.MonitoringCenter.Management.DataAccess.FlightSheet;

using FlightSheet = Core.FlightSheet;

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

    public IAsyncEnumerable<FlightSheet> GetAllAvailable(Guid userId)
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