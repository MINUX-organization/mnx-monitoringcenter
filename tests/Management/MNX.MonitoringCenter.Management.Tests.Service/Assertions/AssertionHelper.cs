using NUnit.Framework;

namespace MNX.MonitoringCenter.Management.Tests.Service.Assertions;

internal static class AssertionHelper
{
    internal static bool AssertNullConsistency(object? expected, object? checking)
    {
        if (expected is null)
        {
            Assert.That(checking, Is.Null);
            return false;
        }

        Assert.That(checking, Is.Not.Null);
        return true;
    }
}
