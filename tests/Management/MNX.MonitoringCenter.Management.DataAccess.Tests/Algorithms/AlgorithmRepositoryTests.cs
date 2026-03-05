using MNX.MonitoringCenter.Management.DataAccess.Algorithm;
using MNX.MonitoringCenter.Management.DataAccess.Tests.Infrastructure;
using MNX.MonitoringCenter.Management.UseCases.Mining.Algorithm;

namespace MNX.MonitoringCenter.Management.DataAccess.Tests.Algorithms;

public partial class AlgorithmRepositoryTests : BaseTest
{
    private IAlgorithmRepository _algorithmRepository;

    [SetUp]
    public void SetUp()
    {
        _algorithmRepository = new AlgorithmRepository(Context);
    }
}
