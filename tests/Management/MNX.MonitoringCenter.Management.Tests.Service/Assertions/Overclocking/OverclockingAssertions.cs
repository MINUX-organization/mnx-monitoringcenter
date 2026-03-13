using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.Core.Overclocking.Cpu;
using MNX.MonitoringCenter.Management.Core.Overclocking.Gpu;
using NUnit.Framework;

namespace MNX.MonitoringCenter.Management.Tests.Service.Assertions.Overclocking;

public static partial class OverclockingAssertions
{
    public static void ShouldBeEquivalentTo(this IEnumerable<IOverclocking?>? checking,
        IEnumerable<IOverclocking?>? expected)
    {
        if (!AssertionHelper.AssertNullConsistency(expected, checking)) return;

        var expectedList = expected!.Where(x => x is not null)
            .OrderBy(x => x!.Id)
            .ToList();
        var checkingList = checking!.Where(x => x is not null)
            .OrderBy(x => x!.Id)
            .ToList();

        Assert.That(checking, Has.Count.EqualTo(expectedList.Count));
        for (var i = 0; i < expectedList.Count; i++)
        {
            checkingList[i].ShouldBeEquivalentTo(expectedList[i]);
        }
    }

    public static void ShouldBeEquivalentTo(this IOverclocking? checking, IOverclocking? expected)
    {
        if (!AssertionHelper.AssertNullConsistency(expected, checking)) return;

        Assert.Multiple(() =>
        {
            Assert.That(checking!.Id, Is.EqualTo(expected!.Id));
            Assert.That(checking.TargetDeviceType, Is.EqualTo(expected.TargetDeviceType));
        });

        switch (expected, checking)
        {
            case (NvidiaGpuOverclocking e, NvidiaGpuOverclocking c):
                AssertNvidiaGpu(e, c);
                break;
            case (AmdGpuOverclocking e, AmdGpuOverclocking c):
                AssertAmdGpu(e, c);
                break;
            case (IntelGpuOverclocking e, IntelGpuOverclocking c):
                AssertIntelGpu(e, c);
                break;
            case (CpuOverclocking e, CpuOverclocking c):
                AssertCpu(e, c);
                break;
        }
    }
}
