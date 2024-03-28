using MNX.MonitoringCenter.Management.UseCases.Commands.Pools;

namespace MNX.MonitoringCenter.Management.UseCases.Tests.Commands.Pools;

public static class PoolCommandTestCase
{
    public static IEnumerable<PoolInputModel> CreateCorrectPoolModel()
    {
        yield return new PoolInputModel("domain", 1323, Guid.NewGuid());
    }

    public static IEnumerable<PoolInputModel> CreateIncorrectPoolModel()
    {
        yield return new PoolInputModel("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", 
                                        -1, Guid.NewGuid());
    }
}
