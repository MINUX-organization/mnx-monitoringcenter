using MNX.MonitoringCenter.Management.Tests.Service.Assertions.FlightSheet;
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

        checkingFlightSheets.ShouldBeEqualTo(query);
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
}
