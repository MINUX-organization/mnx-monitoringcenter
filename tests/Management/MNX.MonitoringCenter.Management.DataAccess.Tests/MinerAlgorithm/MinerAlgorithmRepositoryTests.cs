using MNX.MonitoringCenter.Management.DataAccess.Algorithm;
using MNX.MonitoringCenter.Management.DataAccess.Miner;
using MNX.MonitoringCenter.Management.DataAccess.MinerAlgorithm;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders;
using MNX.MonitoringCenter.Management.UseCases.Mining.Algorithm;
using MNX.MonitoringCenter.Management.UseCases.Mining.Miner;

namespace MNX.MonitoringCenter.Management.DataAccess.Tests.MinerAlgorithm;

using MinerAlgorithm = Core.Mining.Miner.MinerAlgorithm;

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

    private async Task PrepareDataBase(Guid algorithmId, List<MinerAlgorithm> data)
    {
        await _algorithmRepository.AddAsync(new AlgorithmBuilder()
                .WithId(algorithmId)
                .Build());

        foreach (var item in data)
        {
            await _minerRepository.Add(new MinerBuilder()
                .WithId(item.MinerId)
                .Build(),
                default);
        }
    }

    private static class MinerAlgorithmsTestCaseSource
    {
        public static Guid AlgorithmId = Guid.NewGuid();

        public static IEnumerable<List<MinerAlgorithm>> MinerAlgorithmsWithAlgorithmId
        {
            get
            {
                yield return
                [
                    new MinerAlgorithmBuilder()
                        .WithAlgorithmId(AlgorithmId)
                        .Build(),
                    new MinerAlgorithmBuilder()
                        .WithAlgorithmId(AlgorithmId)
                        .Build(),
                    new MinerAlgorithmBuilder()
                        .WithAlgorithmId(AlgorithmId)
                        .Build(),
                    new MinerAlgorithmBuilder()
                        .WithAlgorithmId(AlgorithmId)
                        .Build(),
                ];
            }
        }
    }
}
