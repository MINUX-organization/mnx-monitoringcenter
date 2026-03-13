using MNX.MonitoringCenter.Management.DataAccess.Algorithm;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders;
using MNX.MonitoringCenter.Management.UseCases.Mining.Algorithm;

namespace MNX.MonitoringCenter.Management.DataAccess.Tests.Algorithm;

using Algorithm = Core.Mining.Algorithm;

public partial class AlgorithmRepositoryTests : BaseTest
{
    private IAlgorithmRepository _algorithmRepository;

    [SetUp]
    public void SetUp()
    {
        _algorithmRepository = new AlgorithmRepository(Context);
    }

    private static class AlgorithmsTestCaseSource
    {
        public static Guid UserId = Guid.NewGuid();

        public static IEnumerable<List<Algorithm>> CoreAlgorithmLists
        {
            get
            {
                yield return new List<Algorithm>
                {
                    new AlgorithmBuilder()
                        .WithOwner(UserId)
                        .Build(),
                    new AlgorithmBuilder()
                        .WithOwner(UserId)
                        .Build(),
                    new AlgorithmBuilder()
                        .Build(),
                };
            }
        }
    }
}
