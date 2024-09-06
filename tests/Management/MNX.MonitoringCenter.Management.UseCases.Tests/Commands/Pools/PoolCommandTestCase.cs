using MNX.MonitoringCenter.Management.UseCases.Pool.Commands;

namespace MNX.MonitoringCenter.Management.UseCases.Tests.Commands.Pools;

public static class PoolCommandTestCase
{
    public static IEnumerable<PoolInputModel> CreateCorrectPoolModel()
    {
        yield return new PoolInputModel("domain", 1323, Guid.NewGuid());
    }

    public static IEnumerable<PoolInputModel> CreateIncorrectPoolModel()
    {
        yield return new PoolInputModel("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", 
                                        900000, Guid.NewGuid());
    }
}
