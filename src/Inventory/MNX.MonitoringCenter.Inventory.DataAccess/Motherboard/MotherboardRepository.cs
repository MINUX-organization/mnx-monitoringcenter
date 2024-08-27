using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Inventory.UseCases.Motherboard;

namespace MNX.MonitoringCenter.Inventory.DataAccess.Motherboard;

public class MotherboardRepository : IMotherboardRepository
{
    private readonly Context _context;

    public MotherboardRepository(Context context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public Task<Contracts.Motherboard.Motherboard?> GetByRigId(Guid rigId, CancellationToken cancellationToken)
    {
        return _context.Inventory.AsNoTrackingWithIdentityResolution()
                                 .Where(x => x.RigId == rigId)
                                 .Include(x => x.Motherboard)
                                 .Select(x => x.Motherboard)
                                 .FirstOrDefaultAsync(cancellationToken);

    }
}
