using MNX.MonitoringCenter.Inventory.UseCases.Cpu;

namespace MNX.MonitoringCenter.Inventory.DataAccess.Cpu;

public class CpuRepository : ICpuRepository
{
    private readonly Context _context;

    public CpuRepository(Context context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public IAsyncEnumerable<Contracts.Cpu.Cpu> GetList()
    {
        throw new NotImplementedException();
    }
}
