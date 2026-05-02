using MNX.MonitoringCenter.Management.Contracts.Presets;
using MNX.MonitoringCenter.Management.Tests.Service.Assertions.Overclocking;
using MNX.MonitoringCenter.Management.UseCases.Overclocking.Presets.Commands;
using NUnit.Framework;

namespace MNX.MonitoringCenter.Management.Tests.Service.Assertions;

using Preset = Core.Overclocking.Preset;

public static class PresetAssertions
{
    public static void ShouldBeEqualTo(this IEnumerable<Preset?>? checking, IEnumerable<Preset?>? expected)
    {
        if (!AssertionHelper.AssertNullConsistency(expected, checking)) return;

        var expectedList = expected!
            .Where(x => x is not null)
            .OrderBy(x => x!.Id)
            .ToList();
        var checkingList = checking!
            .Where(x => x is not null)
            .OrderBy(x => x!.Id)
            .ToList();

        Assert.That(checkingList, Has.Count.EqualTo(expectedList.Count));

        for (var i = 0; i < expectedList.Count; i++)
        {
            checkingList[i].ShouldBeEqualTo(expectedList[i]);
        }
    }

    public static void ShouldBeEqualTo(this IEnumerable<PresetModel?>? checking, IEnumerable<Preset?>? expected)
    {
        if (!AssertionHelper.AssertNullConsistency(expected, checking)) return;

        var expectedList = expected!
            .Where(x => x is not null)
            .OrderBy(x => x!.Id)
            .ToList();
        var checkingList = checking!
            .Where(x => x is not null)
            .OrderBy(x => x!.Id)
            .ToList();

        Assert.That(checkingList, Has.Count.EqualTo(expectedList.Count));

        for (var i = 0; i < expectedList.Count; i++)
        {
            checkingList[i].ShouldBeEqualTo(expectedList[i]);
        }
    }

    public static void ShouldBeEqualTo(this Preset? checking, Preset? expected)
    {
        if (!AssertionHelper.AssertNullConsistency(expected, checking)) return;

        Assert.Multiple(() =>
        {
            Assert.That(checking!.Id, Is.EqualTo(expected!.Id));
            Assert.That(checking.Name, Is.EqualTo(expected.Name));
            Assert.That(checking.DeviceName, Is.EqualTo(expected.DeviceName));
            Assert.That(checking.IsVisible, Is.EqualTo(expected.IsVisible));
            Assert.That(checking.OwnerId, Is.EqualTo(expected.OwnerId));
            Assert.That(checking.OverclockingId, Is.EqualTo(expected.OverclockingId));
            checking.Overclocking.ShouldBeEquivalentTo(expected.Overclocking);
        });
    }

    public static void ShouldBeEqualTo(this Preset? checking, PresetInputModel? expected, Guid expectedOwnerId)
    {
        if (!AssertionHelper.AssertNullConsistency(expected, checking)) return;

        Assert.That(checking.Overclocking, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(checking.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(checking.Name, Is.EqualTo(expected.Name));
            Assert.That(checking.DeviceName, Is.EqualTo(expected.DeviceName));
            Assert.That(checking.OwnerId, Is.EqualTo(expectedOwnerId));
            Assert.That(checking.IsVisible, Is.True);
            Assert.That(checking.OverclockingId, Is.Not.EqualTo(Guid.Empty));

            Assert.That(checking.Overclocking.Id, Is.Not.EqualTo(Guid.Empty));
        });
    }

    public static void ShouldBeEqualTo(this Preset? checking, PresetInputModel? expected, Preset originalPreset)
    {
        if (!AssertionHelper.AssertNullConsistency(expected, checking)) return;

        Assert.That(checking.Overclocking, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(checking.Id, Is.EqualTo(originalPreset.Id));
            Assert.That(checking.Name, Is.EqualTo(expected.Name));
            Assert.That(checking.DeviceName, Is.EqualTo(expected.DeviceName));
            Assert.That(checking.OwnerId, Is.EqualTo(originalPreset.OwnerId));
            Assert.That(checking.IsVisible, Is.EqualTo(originalPreset.IsVisible));
            Assert.That(checking.OverclockingId, Is.EqualTo(originalPreset.OverclockingId));

            Assert.That(checking.Id, Is.Not.EqualTo(Guid.Empty));
        });
    }

    public static void ShouldBeEqualTo(this PresetModel? checking, Preset? expected)
    {
        if (!AssertionHelper.AssertNullConsistency(expected, checking)) return;

        Assert.Multiple(() =>
        {
            Assert.That(checking.Id, Is.EqualTo(expected.Id));
            Assert.That(checking.Name, Is.EqualTo(expected.Name));
            Assert.That(checking.DeviceName, Is.EqualTo(expected.DeviceName));
            Assert.That(checking.Overclocking.TargetDeviceType, Is.EqualTo(expected.Overclocking.TargetDeviceType));
        });
    }
}
