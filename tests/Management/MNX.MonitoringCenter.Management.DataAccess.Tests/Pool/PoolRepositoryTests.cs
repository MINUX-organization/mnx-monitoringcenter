using MNX.MonitoringCenter.Management.DataAccess.Pool;
using MNX.MonitoringCenter.Management.UseCases.Mining.Pool;

namespace MNX.MonitoringCenter.Management.DataAccess.Tests.Pool;

public partial class PoolRepositoryTests : BaseTest
{
    private IPoolRepository _poolRepository;

    [SetUp]
    public void SetUp()
    {
        _poolRepository = new PoolRepository(Context);
    }
}
