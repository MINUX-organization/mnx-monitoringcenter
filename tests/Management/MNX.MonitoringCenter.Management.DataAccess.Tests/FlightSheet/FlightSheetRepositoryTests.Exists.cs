namespace MNX.MonitoringCenter.Management.DataAccess.Tests.FlightSheet;

using FlightSheet = Core.Mining.FlightSheet.FlightSheet;

public partial class FlightSheetRepositoryTests
{
    [TestCaseSource(typeof(FlightSheetsTestCaseSource), nameof(FlightSheetsTestCaseSource.FlightSheets))]
    public async Task ExistsAvailable_ValidNameAndUserId_ReturnsTrue(FlightSheet data)
    {
        // Arrange

        var name = data.Name;
        var userId = FlightSheetsTestCaseSource.UserId;

        await _flightSheetRepository.Add(data);


        // Act

        var isExists = await _flightSheetRepository
            .ExistsAvailable(name, userId, default);


        // Assert

        Assert.That(isExists, Is.True);
    }

    [TestCaseSource(typeof(FlightSheetsTestCaseSource), nameof(FlightSheetsTestCaseSource.FlightSheets))]
    public async Task ExistsAvailable_InvalidNameAndUserId_ReturnsFalse(FlightSheet data)
    {
        // Arrange

        var name = "SomeInvalidFlightSheetName";
        var userId = Guid.NewGuid();

        await _flightSheetRepository.Add(data);


        // Act

        var isExists = await _flightSheetRepository.ExistsAvailable(name, userId, default);


        // Assert

        Assert.That(isExists, Is.False);
    }

    [TestCaseSource(typeof(FlightSheetsTestCaseSource), nameof(FlightSheetsTestCaseSource.FlightSheets))]
    public async Task ExistsAvailable_ValidId_ReturnsTrue(FlightSheet data)
    {
        // Arrage

        var id = data.Id;

        await _flightSheetRepository.Add(data);
        
        
        // Act

        var isExists = await _flightSheetRepository
            .Exists(id, default);


        // Assert

        Assert.That(isExists, Is.True);
    }

    [TestCaseSource(typeof(FlightSheetsTestCaseSource), nameof(FlightSheetsTestCaseSource.FlightSheets))]
    public async Task ExistsAvailable_InvalidId_ReturnsFalse(FlightSheet data)
    {
        // Arrage

        var id = Guid.NewGuid();

        await _flightSheetRepository.Add(data);


        // Act

        var isExists = await _flightSheetRepository
            .Exists(id, default);


        // Assert

        Assert.That(isExists, Is.False);
    }
}
