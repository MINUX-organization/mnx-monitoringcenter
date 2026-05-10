using FluentAssertions;
using NUnit.Framework;

namespace MNX.MonitoringCenter.Management.Tests.Service.Assertions;

using Algorithm = Core.Mining.Algorithm;

public static class AlgorithmAssertions
{
    public static void ShouldBeEqualTo(this IEnumerable<Algorithm?>? checking, IEnumerable<Algorithm?>? expected)
    {
        if (expected is null)
        {
            checking.Should().BeNull();
            return;
        }

        checking.Should().NotBeNull();
        var expectedList = expected
            .Where(x => x is not null)
            .OrderBy(x => x!.Id)
            .ToList();
        var checkingList = checking
            .Where(x => x is not null)
            .OrderBy(x => x!.Id)
            .ToList();

        Assert.That(checking, Has.Count.EqualTo(expectedList.Count));
        for (var i = 0; i < expectedList.Count; i++)
        {
            checkingList[i].ShouldBeEqualTo(expectedList[i]);
        }
    }

    public static void ShouldBeEqualTo(this Algorithm? checking, Algorithm? expected)
    {
        if (expected is null)
        {
            checking.Should().BeNull();
            return;
        }

        checking.Should().NotBeNull();
        checking.Should().Satisfy<Algorithm>(x =>
        {
            x.Id.Should().Be(expected.Id);
            x.Name.Should().Be(expected.Name);
            x.OwnerId.Should().Be(expected.OwnerId);
        });
    }
}
