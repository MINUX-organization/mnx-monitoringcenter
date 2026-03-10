using MNX.MonitoringCenter.Management.DataAccess.Algorithm;
using MNX.MonitoringCenter.Management.DataAccess.Miner;
using MNX.MonitoringCenter.Management.DataAccess.MinerAlgorithm;
using MNX.MonitoringCenter.Management.UseCases.Mining.Algorithm;
using MNX.MonitoringCenter.Management.UseCases.Mining.Miner;

namespace MNX.MonitoringCenter.Management.DataAccess.Tests.MinerAlgorithm;

public partial class MinerAlgorithmRepositoryTests : BaseTest
{
    private IMinerAlgorithmRepository _minerAlgorithmRepository;
    private IMinerRepository _minerRepository;
    private IAlgorithmRepository _algorithmRepository;

    [SetUp]
    public void SetUp()
    {
        _minerAlgorithmRepository = new MinerAlgorithmRepository(Context);
        _minerRepository = new MinerRepository(Context);
        _algorithmRepository = new AlgorithmRepository(Context);
    }
}
