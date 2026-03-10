using MNX.MonitoringCenter.Management.UseCases;

namespace MNX.MonitoringCenter.Management.DataAccess.Tests.FlightSheet;

using FlightSheet = Core.Mining.FlightSheet.FlightSheet;

public partial class FlightSheetRepositoryTests
{
    [TestCaseSource(typeof(FlightSheetsTestCaseSource), nameof(FlightSheetsTestCaseSource.FlightSheetLists))]
    public async Task GetAllAvailable_ValidUserId_ReturnsEntitiesList(List<FlightSheet> data)
    {
        // Arrange

        var specification = new Specification(FlightSheetsTestCaseSource.UserId);
        var query = data.Where(x => x.OwnerId == specification.UserId);
        var expectedCount = query.Count();
        await PrepareDataBase(data);


        // Act

        var checkingFlightSheets = await _flightSheetRepository
            .GetAllAvailable(specification).ToListAsync();


        // Assert

        Assert.That(checkingFlightSheets, Is.Not.Null);
        Assert.That(checkingFlightSheets, Has.Count.EqualTo(expectedCount));
        AssertFlightSheets([.. query], checkingFlightSheets);
    }

    [TestCaseSource(typeof(FlightSheetsTestCaseSource), nameof(FlightSheetsTestCaseSource.FlightSheetLists))]
    public async Task GetAllAvailable_InvalidUserId_ReturnsEmptyList(List<FlightSheet> data)
    {
        // Arrange
        
        var specification = new Specification(Guid.NewGuid());
        var query = data.Where(x => x.OwnerId == specification.UserId);
        var expectedCount = query.Count();
        await PrepareDataBase(data);

        
        // Act

        var checkingFlightSheets = await _flightSheetRepository
            .GetAllAvailable(specification).ToListAsync();


        // Assert

        Assert.That(checkingFlightSheets, Is.Not.Null);
        Assert.That(checkingFlightSheets, Has.Count.EqualTo(expectedCount));
    }

    private static void AssertFlightSheets(List<FlightSheet> expected, List<FlightSheet> checking)
    {
        expected = [.. expected.OrderBy(x => x.Name)];
        checking = [.. checking.OrderBy(x => x.Name)];

        for (var i = 0; i < expected.Count; i++)
        {
            Assert.Multiple(() =>
            {
                Assert.That(checking[i].Id, Is.EqualTo(expected[i].Id));
                Assert.That(checking[i].OwnerId, Is.EqualTo(expected[i].OwnerId));
                Assert.That(checking[i].Name, Is.EqualTo(expected[i].Name));
                AssertTargets(expected[i].Targets, checking[i].Targets);
            });
        }
    }

    private async Task PrepareDataBase(List<FlightSheet> data)
    {
        foreach (var item in data)
        {
            await _flightSheetRepository.Add(item);
        }
    }
}
