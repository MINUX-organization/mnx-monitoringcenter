using MNX.MonitoringCenter.Management.Tests.Service.Assertions.FlightSheet;

namespace MNX.MonitoringCenter.Management.DataAccess.Tests.FlightSheet;

using FlightSheet = Core.Mining.FlightSheet.FlightSheet;

public partial class FlightSheetRepositoryTests
{
    [TestCaseSource(typeof(FlightSheetsTestCaseSource), nameof(FlightSheetsTestCaseSource.FlightSheets))]
    public async Task GetAvailableById_ValidIdAndUserId_ReturnsEntity(FlightSheet data)
    {
        // Arrange

        var flightSheetId = data.Id;
        var userId = FlightSheetsTestCaseSource.UserId;

        await _flightSheetRepository.Add(data);


        // Act

        var checkingFlightSheet = await _flightSheetRepository
            .GetAvailableById(flightSheetId, userId, default);


        // Assert

        checkingFlightSheet.ShouldBeEqualTo(data);
    }

    [TestCaseSource(typeof(FlightSheetsTestCaseSource), nameof(FlightSheetsTestCaseSource.FlightSheets))]
    public async Task GetAvailableById_InvalidIdAndUserId_ReturnsNull(FlightSheet data)
    {
        // Arrange

        var flightSheetId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        await _flightSheetRepository.Add(data);


        // Act

        var checkingFlightSheet = await _flightSheetRepository
            .GetAvailableById(flightSheetId, userId, default);


        // Assert

        Assert.That(checkingFlightSheet, Is.Null);
    }
}
