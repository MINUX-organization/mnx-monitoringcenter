using MNX.MonitoringCenter.Inventory.Contracts.Devices.Cpu;
using MNX.MonitoringCenter.Management.Contracts.Overclocking;
using MNX.MonitoringCenter.Management.Contracts.Overclocking.Cpu;
using MNX.MonitoringCenter.Management.Contracts.Overclocking.Gpu;
using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.Core.Overclocking.Cpu;
using MNX.MonitoringCenter.Management.Core.Overclocking.Enums;
using MNX.MonitoringCenter.Management.Core.Overclocking.Gpu.Fan;
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
            case (Core.Overclocking.Gpu.NvidiaGpuOverclocking e, Core.Overclocking.Gpu.NvidiaGpuOverclocking c):
                AssertNvidiaGpu(e, c);
                break;
            case (Core.Overclocking.Gpu.AmdGpuOverclocking e, Core.Overclocking.Gpu.AmdGpuOverclocking c):
                AssertAmdGpu(e, c);
                break;
            case (Core.Overclocking.Gpu.IntelGpuOverclocking e, Core.Overclocking.Gpu.IntelGpuOverclocking c):
                AssertIntelGpu(e, c);
                break;
            case (Core.Overclocking.Cpu.CpuOverclocking e, Core.Overclocking.Cpu.CpuOverclocking c):
                AssertCpu(e, c);
                break;
        }
    }

    public static void ShouldBeEquivalentTo(this IOverclocking? checking, Inventory.Contracts.Devices.Gpu.Overclocking.NvidiaGpuOverclocking? expected)
    {
        if (!AssertionHelper.AssertNullConsistency(expected, checking)) return;

        Assert.That(checking, Is.TypeOf<Core.Overclocking.Gpu.NvidiaGpuOverclocking>());
        Assert.Multiple(() =>
        {
            Assert.That(checking.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(checking.TargetDeviceType, Is.EqualTo(OverclockingTargetDeviceType.NvidiaGPU));

            var coreGpuOverclocking = (Core.Overclocking.Gpu.NvidiaGpuOverclocking)checking;
            Assert.That(coreGpuOverclocking.PowerLimit, Is.EqualTo(expected.PowerLimit));
            Assert.That(coreGpuOverclocking.CoreClockLock, Is.EqualTo(expected.CoreClockLock));
            Assert.That(coreGpuOverclocking.CoreClockOffset, Is.EqualTo(expected.CoreClockOffset));
            Assert.That(coreGpuOverclocking.CoreVoltage, Is.EqualTo(expected.CoreVoltage));
            Assert.That(coreGpuOverclocking.CoreVoltageOffset, Is.EqualTo(expected.CoreVoltageOffset));
            Assert.That(coreGpuOverclocking.MemoryClockLock, Is.EqualTo(expected.MemoryClockLock));
            Assert.That(coreGpuOverclocking.MemoryClockOffset, Is.EqualTo(expected.MemoryClockOffset));
            Assert.That(coreGpuOverclocking.MemoryVoltage, Is.EqualTo(expected.MemoryVoltage));
            Assert.That(coreGpuOverclocking.MemoryVoltageOffset, Is.EqualTo(expected.MemoryVoltageOffset));

            Assert.That(coreGpuOverclocking.FanOverclocking, Is.Not.Null);
            Assert.That(coreGpuOverclocking.FanOverclocking.Type, Is.EqualTo(FanOverclockingType.TargetSpeed));
            Assert.That(coreGpuOverclocking.FanOverclocking, Is.TypeOf<FanOverclockingWithTargetSpeed>());

            var coreFanOverclocking = (FanOverclockingWithTargetSpeed)coreGpuOverclocking.FanOverclocking;
            Assert.That(coreFanOverclocking.TargetSpeed, Is.EqualTo(expected.FanSpeed));
        });
    }

    public static void ShouldBeEquivalentTo(this Inventory.Contracts.Devices.Overclocking? checking, Core.Overclocking.Gpu.NvidiaGpuOverclocking? expected)
    {
        if (!AssertionHelper.AssertNullConsistency(expected, checking)) return;

        Assert.That(checking, Is.TypeOf<Inventory.Contracts.Devices.Gpu.Overclocking.NvidiaGpuOverclocking>());
        Assert.Multiple(() =>
        {
            var inventoryGpuOverclocking = (Inventory.Contracts.Devices.Gpu.Overclocking.NvidiaGpuOverclocking)checking;
            Assert.That(inventoryGpuOverclocking.PowerLimit, Is.EqualTo(expected.PowerLimit));
            Assert.That(inventoryGpuOverclocking.CoreClockLock, Is.EqualTo(expected.CoreClockLock));
            Assert.That(inventoryGpuOverclocking.CoreClockOffset, Is.EqualTo(expected.CoreClockOffset));
            Assert.That(inventoryGpuOverclocking.CoreVoltage, Is.EqualTo(expected.CoreVoltage));
            Assert.That(inventoryGpuOverclocking.CoreVoltageOffset, Is.EqualTo(expected.CoreVoltageOffset));
            Assert.That(inventoryGpuOverclocking.MemoryClockLock, Is.EqualTo(expected.MemoryClockLock));
            Assert.That(inventoryGpuOverclocking.MemoryClockOffset, Is.EqualTo(expected.MemoryClockOffset));
            Assert.That(inventoryGpuOverclocking.MemoryVoltage, Is.EqualTo(expected.MemoryVoltage));
            Assert.That(inventoryGpuOverclocking.MemoryVoltageOffset, Is.EqualTo(expected.MemoryVoltageOffset));
        });
    }

    public static void ShouldBeEquivalentTo(this IOverclocking? checking, Inventory.Contracts.Devices.Gpu.Overclocking.AmdGpuOverclocking? expected)
    {
        if (!AssertionHelper.AssertNullConsistency(expected, checking)) return;

        Assert.That(checking, Is.TypeOf<Core.Overclocking.Gpu.AmdGpuOverclocking>());
        Assert.Multiple(() =>
        {
            Assert.That(checking!.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(checking.TargetDeviceType, Is.EqualTo(OverclockingTargetDeviceType.AmdGPU));

            var coreGpuOverclocking = (Core.Overclocking.Gpu.AmdGpuOverclocking)checking;
            Assert.That(coreGpuOverclocking.CoreClockLock, Is.EqualTo(expected!.CoreClockLock));
            Assert.That(coreGpuOverclocking.CoreClockState, Is.EqualTo(expected.CoreClockState));
            Assert.That(coreGpuOverclocking.CoreVoltage, Is.EqualTo(expected.CoreVoltage));
            Assert.That(coreGpuOverclocking.CoreClockState, Is.EqualTo(expected.CoreClockState));
            Assert.That(coreGpuOverclocking.MemoryClockLock, Is.EqualTo(expected.MemoryClockLock));
            Assert.That(coreGpuOverclocking.MemoryClockState, Is.EqualTo(expected.MemoryClockState));
            Assert.That(coreGpuOverclocking.MemoryVoltage, Is.EqualTo(expected.MemoryVoltage));
            Assert.That(coreGpuOverclocking.MemoryControllerVoltage, Is.EqualTo(expected.MemoryControllerVoltage));
            Assert.That(coreGpuOverclocking.MemoryTweak, Is.EqualTo(expected.MemoryTweak));
            Assert.That(coreGpuOverclocking.EnhancedOverclock, Is.EqualTo(expected.EnhancedOverclock));
            Assert.That(coreGpuOverclocking.SocFrequency, Is.EqualTo(expected.SocFrequency));
            Assert.That(coreGpuOverclocking.SocVoltage, Is.EqualTo(expected.SocVoltage));
            Assert.That(coreGpuOverclocking.PowerLimit, Is.EqualTo(expected.PowerLimit));
            Assert.That(coreGpuOverclocking.AlternativeDownVoltage, Is.EqualTo(expected.AlternativeDownVoltage));
        });
    }

    public static void ShouldBeEquivalentTo(this Inventory.Contracts.Devices.Overclocking? checking, Core.Overclocking.Gpu.AmdGpuOverclocking? expected)
    {
        if (!AssertionHelper.AssertNullConsistency(expected, checking)) return;

        Assert.That(checking, Is.TypeOf<Inventory.Contracts.Devices.Gpu.Overclocking.AmdGpuOverclocking>());
        Assert.Multiple(() =>
        {
            var inventoryGpuOverclocking = (Inventory.Contracts.Devices.Gpu.Overclocking.AmdGpuOverclocking)checking!;
            Assert.That(inventoryGpuOverclocking!.CoreClockLock, Is.EqualTo(expected!.CoreClockLock)); ;
            Assert.That(inventoryGpuOverclocking.CoreClockState, Is.EqualTo(expected.CoreClockState));
            Assert.That(inventoryGpuOverclocking.CoreVoltage, Is.EqualTo(expected.CoreVoltage));
            Assert.That(inventoryGpuOverclocking.CoreClockState, Is.EqualTo(expected.CoreClockState));
            Assert.That(inventoryGpuOverclocking.MemoryClockLock, Is.EqualTo(expected.MemoryClockLock));
            Assert.That(inventoryGpuOverclocking.MemoryClockState, Is.EqualTo(expected.MemoryClockState));
            Assert.That(inventoryGpuOverclocking.MemoryVoltage, Is.EqualTo(expected.MemoryVoltage));
            Assert.That(inventoryGpuOverclocking.MemoryControllerVoltage, Is.EqualTo(expected.MemoryControllerVoltage));
            Assert.That(inventoryGpuOverclocking.MemoryTweak, Is.EqualTo(expected.MemoryTweak));
            Assert.That(inventoryGpuOverclocking.EnhancedOverclock, Is.EqualTo(expected.EnhancedOverclock));
            Assert.That(inventoryGpuOverclocking.SocFrequency, Is.EqualTo(expected.SocFrequency));
            Assert.That(inventoryGpuOverclocking.SocVoltage, Is.EqualTo(expected.SocVoltage));
            Assert.That(inventoryGpuOverclocking.PowerLimit, Is.EqualTo(expected.PowerLimit));
            Assert.That(inventoryGpuOverclocking.AlternativeDownVoltage, Is.EqualTo(expected.AlternativeDownVoltage));
        });
    }

    public static void ShouldBeEquivalentTo(this IOverclocking? checking, Inventory.Contracts.Devices.Cpu.CpuOverclocking? expected)
    {
        if (!AssertionHelper.AssertNullConsistency(expected, checking)) return;

        Assert.That(checking, Is.TypeOf<Core.Overclocking.Cpu.CpuOverclocking>());
        Assert.Multiple(() =>
        {
            Assert.That(checking!.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(checking.TargetDeviceType, Is.EqualTo(OverclockingTargetDeviceType.CPU));

            var coreCpuOverclocking = (Core.Overclocking.Cpu.CpuOverclocking)checking;
            Assert.That(coreCpuOverclocking.CoreClockLock, Is.EqualTo(expected!.CoreClockLock));
            Assert.That(coreCpuOverclocking.CoreVoltage, Is.EqualTo(expected.CoreVoltage));
        });
    }

    public static void ShouldBeEquivalentTo(this Inventory.Contracts.Devices.Overclocking? checking, Core.Overclocking.Cpu.CpuOverclocking? expected)
    {
        if (!AssertionHelper.AssertNullConsistency(expected, checking)) return;

        Assert.That(checking, Is.TypeOf<Inventory.Contracts.Devices.Cpu.CpuOverclocking>());
        Assert.Multiple(() =>
        {
            var inventoryCpuOverclocking = (Inventory.Contracts.Devices.Cpu.CpuOverclocking)checking!;
            Assert.That(inventoryCpuOverclocking.CoreClockLock, Is.EqualTo(expected.CoreClockLock));
            Assert.That(inventoryCpuOverclocking.CoreVoltage, Is.EqualTo(expected.CoreVoltage));
        });
    }

    public static void ShouldBeEquivalentTo(this IOverclocking? checking, AmdGpuOverclockingModel? expected)
    {
        if (!AssertionHelper.AssertNullConsistency(expected, checking)) return;

        Assert.That(checking, Is.TypeOf<Core.Overclocking.Gpu.AmdGpuOverclocking>());
        Assert.Multiple(() =>
        {
            Assert.That(checking!.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(checking.TargetDeviceType, Is.EqualTo(expected!.TargetDeviceType));

            var amdGpuOverclocking = (Core.Overclocking.Gpu.AmdGpuOverclocking)checking;
            Assert.That(amdGpuOverclocking.PowerLimit, Is.EqualTo(expected.PowerLimit));
            Assert.That(amdGpuOverclocking.CoreClockLock, Is.EqualTo(expected.CoreClockLock));
            Assert.That(amdGpuOverclocking.CoreClockState, Is.EqualTo(expected.CoreClockState));
            Assert.That(amdGpuOverclocking.CoreVoltage, Is.EqualTo(expected.CoreVoltage));
            Assert.That(amdGpuOverclocking.CoreVoltageOffset, Is.EqualTo(expected.CoreVoltageOffset));
            Assert.That(amdGpuOverclocking.MemoryClockLock, Is.EqualTo(expected.MemoryClockLock));
            Assert.That(amdGpuOverclocking.MemoryClockState, Is.EqualTo(expected.MemoryClockState));
            Assert.That(amdGpuOverclocking.MemoryVoltage, Is.EqualTo(expected.MemoryVoltage));
            Assert.That(amdGpuOverclocking.MemoryControllerVoltage, Is.EqualTo(expected.MemoryControllerVoltage));
            Assert.That(amdGpuOverclocking.MemoryTweak, Is.EqualTo(expected.MemoryTweak));
            Assert.That(amdGpuOverclocking.EnhancedOverclock, Is.EqualTo(expected.EnhancedOverclock));
            Assert.That(amdGpuOverclocking.AlternativeDownVoltage, Is.EqualTo(expected.AlternativeDownVoltage));
            Assert.That(amdGpuOverclocking.SocFrequency, Is.EqualTo(expected.SocFrequency));
            Assert.That(amdGpuOverclocking.SocVoltage, Is.EqualTo(expected.SocVoltage));
        });
    }

    public static void ShouldBeEquivalentTo(this IOverclocking? checking, AmdGpuOverclockingModel? expected, Guid originalId)
    {
        if (!AssertionHelper.AssertNullConsistency(expected, checking)) return;

        Assert.That(checking, Is.TypeOf<Core.Overclocking.Gpu.AmdGpuOverclocking>());
        Assert.Multiple(() =>
        {
            Assert.That(checking!.Id, Is.EqualTo(originalId));
            Assert.That(checking.TargetDeviceType, Is.EqualTo(expected!.TargetDeviceType));

            var amdGpuOverclocking = (Core.Overclocking.Gpu.AmdGpuOverclocking)checking;
            Assert.That(amdGpuOverclocking.PowerLimit, Is.EqualTo(expected.PowerLimit));
            Assert.That(amdGpuOverclocking.CoreClockLock, Is.EqualTo(expected.CoreClockLock));
            Assert.That(amdGpuOverclocking.CoreClockState, Is.EqualTo(expected.CoreClockState));
            Assert.That(amdGpuOverclocking.CoreVoltage, Is.EqualTo(expected.CoreVoltage));
            Assert.That(amdGpuOverclocking.CoreVoltageOffset, Is.EqualTo(expected.CoreVoltageOffset));
            Assert.That(amdGpuOverclocking.MemoryClockLock, Is.EqualTo(expected.MemoryClockLock));
            Assert.That(amdGpuOverclocking.MemoryClockState, Is.EqualTo(expected.MemoryClockState));
            Assert.That(amdGpuOverclocking.MemoryVoltage, Is.EqualTo(expected.MemoryVoltage));
            Assert.That(amdGpuOverclocking.MemoryControllerVoltage, Is.EqualTo(expected.MemoryControllerVoltage));
            Assert.That(amdGpuOverclocking.MemoryTweak, Is.EqualTo(expected.MemoryTweak));
            Assert.That(amdGpuOverclocking.EnhancedOverclock, Is.EqualTo(expected.EnhancedOverclock));
            Assert.That(amdGpuOverclocking.AlternativeDownVoltage, Is.EqualTo(expected.AlternativeDownVoltage));
            Assert.That(amdGpuOverclocking.SocFrequency, Is.EqualTo(expected.SocFrequency));
            Assert.That(amdGpuOverclocking.SocVoltage, Is.EqualTo(expected.SocVoltage));
        });
    }

    public static void ShouldBeEquivalentTo(this IOverclockingModel? checking, Core.Overclocking.Gpu.AmdGpuOverclocking? expected)
    {
        if (!AssertionHelper.AssertNullConsistency(expected, checking)) return;

        Assert.That(checking, Is.TypeOf<AmdGpuOverclockingModel>());
        Assert.Multiple(() =>
        {
            Assert.That(checking!.TargetDeviceType, Is.EqualTo(expected!.TargetDeviceType));

            var amdGpuOverclockingModel = (AmdGpuOverclockingModel)checking;
            Assert.That(amdGpuOverclockingModel.PowerLimit, Is.EqualTo(expected.PowerLimit));
            Assert.That(amdGpuOverclockingModel.CoreClockLock, Is.EqualTo(expected.CoreClockLock));
            Assert.That(amdGpuOverclockingModel.CoreClockState, Is.EqualTo(expected.CoreClockState));
            Assert.That(amdGpuOverclockingModel.CoreVoltage, Is.EqualTo(expected.CoreVoltage));
            Assert.That(amdGpuOverclockingModel.CoreVoltageOffset, Is.EqualTo(expected.CoreVoltageOffset));
            Assert.That(amdGpuOverclockingModel.MemoryClockLock, Is.EqualTo(expected.MemoryClockLock));
            Assert.That(amdGpuOverclockingModel.MemoryClockState, Is.EqualTo(expected.MemoryClockState));
            Assert.That(amdGpuOverclockingModel.MemoryVoltage, Is.EqualTo(expected.MemoryVoltage));
            Assert.That(amdGpuOverclockingModel.MemoryControllerVoltage, Is.EqualTo(expected.MemoryControllerVoltage));
            Assert.That(amdGpuOverclockingModel.MemoryTweak, Is.EqualTo(expected.MemoryTweak));
            Assert.That(amdGpuOverclockingModel.EnhancedOverclock, Is.EqualTo(expected.EnhancedOverclock));
            Assert.That(amdGpuOverclockingModel.AlternativeDownVoltage, Is.EqualTo(expected.AlternativeDownVoltage));
            Assert.That(amdGpuOverclockingModel.SocFrequency, Is.EqualTo(expected.SocFrequency));
            Assert.That(amdGpuOverclockingModel.SocVoltage, Is.EqualTo(expected.SocVoltage));
        });
    }

    public static void ShouldBeEquivalentTo(this IOverclocking? checking, NvidiaGpuOverclockingModel? expected)
    {
        if (!AssertionHelper.AssertNullConsistency(expected, checking)) return;

        Assert.That(checking, Is.TypeOf<Core.Overclocking.Gpu.NvidiaGpuOverclocking>());
        Assert.Multiple(() =>
        {
            Assert.That(checking!.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(checking.TargetDeviceType, Is.EqualTo(expected!.TargetDeviceType));

            var nvidiaGpuOverclocking = (Core.Overclocking.Gpu.NvidiaGpuOverclocking)checking;
            Assert.That(nvidiaGpuOverclocking.CoreClockLock, Is.EqualTo(expected.CoreClockLock));
            Assert.That(nvidiaGpuOverclocking.CoreClockOffset, Is.EqualTo(expected.CoreClockOffset));
            Assert.That(nvidiaGpuOverclocking.PowerLimit, Is.EqualTo(expected.PowerLimit));
            Assert.That(nvidiaGpuOverclocking.CoreVoltage, Is.EqualTo(expected.CoreVoltage));
            Assert.That(nvidiaGpuOverclocking.CoreVoltageOffset, Is.EqualTo(expected.CoreVoltageOffset));
            Assert.That(nvidiaGpuOverclocking.MemoryClockLock, Is.EqualTo(expected.MemoryClockLock));
            Assert.That(nvidiaGpuOverclocking.MemoryClockOffset, Is.EqualTo(expected.MemoryClockOffset));
            Assert.That(nvidiaGpuOverclocking.MemoryVoltage, Is.EqualTo(expected.MemoryVoltage));
            Assert.That(nvidiaGpuOverclocking.MemoryVoltageOffset, Is.EqualTo(expected.MemoryVoltageOffset));
        });
    }

    public static void ShouldBeEquivalentTo(this IOverclocking? checking, Core.Overclocking.Gpu.NvidiaGpuOverclocking? expected, Guid originalId)
    {
        if (!AssertionHelper.AssertNullConsistency(expected, checking)) return;

        Assert.That(checking, Is.TypeOf<Core.Overclocking.Gpu.NvidiaGpuOverclocking>());
        Assert.Multiple(() =>
        {
            Assert.That(checking!.Id, Is.EqualTo(originalId));
            Assert.That(checking.TargetDeviceType, Is.EqualTo(expected!.TargetDeviceType));

            var nvidiaGpuOverclocking = (Core.Overclocking.Gpu.NvidiaGpuOverclocking)checking;
            Assert.That(nvidiaGpuOverclocking.PowerLimit, Is.EqualTo(expected.PowerLimit));
            Assert.That(nvidiaGpuOverclocking.CoreClockLock, Is.EqualTo(expected.CoreClockLock));
            Assert.That(nvidiaGpuOverclocking.CoreClockOffset, Is.EqualTo(expected.CoreClockOffset));
            Assert.That(nvidiaGpuOverclocking.CoreVoltage, Is.EqualTo(expected.CoreVoltage));
            Assert.That(nvidiaGpuOverclocking.CoreVoltageOffset, Is.EqualTo(expected.CoreVoltageOffset));
            Assert.That(nvidiaGpuOverclocking.MemoryClockLock, Is.EqualTo(expected.MemoryClockLock));
            Assert.That(nvidiaGpuOverclocking.MemoryClockOffset, Is.EqualTo(expected.MemoryClockOffset));
            Assert.That(nvidiaGpuOverclocking.MemoryVoltage, Is.EqualTo(expected.MemoryVoltage));
            Assert.That(nvidiaGpuOverclocking.MemoryVoltageOffset, Is.EqualTo(expected.MemoryVoltageOffset));
        });
    }

    public static void ShouldBeEquivalentTo(this IOverclockingModel? checking, Core.Overclocking.Gpu.NvidiaGpuOverclocking? expected)
    {
        if (!AssertionHelper.AssertNullConsistency(expected, checking)) return;

        Assert.That(checking, Is.Not.Null);
        Assert.That(checking, Is.TypeOf<NvidiaGpuOverclockingModel>());
        Assert.Multiple(() =>
        {
            Assert.That(checking!.TargetDeviceType, Is.EqualTo(expected!.TargetDeviceType));

            var nvidiaGpuOverclockingModel = (NvidiaGpuOverclockingModel)checking;
            Assert.That(nvidiaGpuOverclockingModel.PowerLimit, Is.EqualTo(expected.PowerLimit));
            Assert.That(nvidiaGpuOverclockingModel.CoreClockLock, Is.EqualTo(expected.CoreClockLock));
            Assert.That(nvidiaGpuOverclockingModel.CoreClockOffset, Is.EqualTo(expected.CoreClockOffset));
            Assert.That(nvidiaGpuOverclockingModel.CoreVoltage, Is.EqualTo(expected.CoreVoltage));
            Assert.That(nvidiaGpuOverclockingModel.CoreVoltageOffset, Is.EqualTo(expected.CoreVoltageOffset));
            Assert.That(nvidiaGpuOverclockingModel.MemoryClockLock, Is.EqualTo(expected.MemoryClockLock));
            Assert.That(nvidiaGpuOverclockingModel.MemoryClockOffset, Is.EqualTo(expected.MemoryClockOffset));
            Assert.That(nvidiaGpuOverclockingModel.MemoryVoltage, Is.EqualTo(expected.MemoryVoltage));
            Assert.That(nvidiaGpuOverclockingModel.MemoryVoltage, Is.EqualTo(expected.MemoryVoltage));
        });
    }

    public static void ShouldBeEquivalentTo(this IOverclocking? checking, CpuOverclockingModel? expected)
    {
        if (!AssertionHelper.AssertNullConsistency(expected, checking)) return;

        Assert.Multiple(() =>
        {
            Assert.That(checking!.TargetDeviceType, Is.EqualTo(expected!.TargetDeviceType));
            Assert.That(checking, Is.TypeOf<Core.Overclocking.Cpu.CpuOverclocking>());

            var cpuOverclocking = (Core.Overclocking.Cpu.CpuOverclocking)checking;
            Assert.That(cpuOverclocking.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(cpuOverclocking.CoreClockLock, Is.EqualTo(expected.CoreClockLock));
            Assert.That(cpuOverclocking.CoreVoltage, Is.EqualTo(expected.CoreVoltage));
        });
    }

    public static void ShouldBeEquivalentTo(this IOverclocking? checking, CpuOverclockingModel? expected, Guid originalId)
    {
        if (!AssertionHelper.AssertNullConsistency(expected, checking)) return;

        Assert.Multiple(() =>
        {
            Assert.That(checking!.TargetDeviceType, Is.EqualTo(expected!.TargetDeviceType));
            Assert.That(checking, Is.TypeOf<Core.Overclocking.Cpu.CpuOverclocking>());

            var cpuOverclocking = (Core.Overclocking.Cpu.CpuOverclocking)checking;
            Assert.That(cpuOverclocking.Id, Is.EqualTo(originalId));
            Assert.That(cpuOverclocking.CoreClockLock, Is.EqualTo(expected.CoreClockLock));
            Assert.That(cpuOverclocking.CoreVoltage, Is.EqualTo(expected.CoreVoltage));
        });
    }

    public static void ShouldBeEquivalentTo(this IOverclockingModel? checking, Core.Overclocking.Cpu.CpuOverclocking? expected)
    {
        if (!AssertionHelper.AssertNullConsistency(expected, checking)) return;

        Assert.That(checking, Is.TypeOf<CpuOverclockingModel>());
        Assert.Multiple(() =>
        {
            Assert.That(checking!.TargetDeviceType, Is.EqualTo(expected!.TargetDeviceType));

            var cpuOverclockingModel = (CpuOverclockingModel)checking;
            Assert.That(cpuOverclockingModel.CoreClockLock, Is.EqualTo(expected.CoreClockLock));
            Assert.That(cpuOverclockingModel.CoreVoltage, Is.EqualTo(expected.CoreVoltage));
        });
    }
}
