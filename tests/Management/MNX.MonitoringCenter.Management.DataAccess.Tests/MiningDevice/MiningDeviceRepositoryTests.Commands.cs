using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders.Overclockings;
using MNX.MonitoringCenter.Management.UseCases;

namespace MNX.MonitoringCenter.Management.DataAccess.Tests.MiningDevice;

public partial class MiningDeviceRepositoryTests
{
    [TestCaseSource(typeof(MiningDevicesTestCaseSource), nameof(MiningDevicesTestCaseSource.MiningDeviceListsWithVisiblePreset))]
    public async Task SetPreset_ValidPresetIdAndDeviceIds_ShouldSetPresetToDevices(List<MiningDeviceInfo> data)
    {
        // Arrange

        var deviceIds = data.Select(x => x.Id);
        var presetId = Guid.NewGuid();
        var userId = MiningDevicesTestCaseSource.UserId;

        await PrepareDataBase(data);
        await _presetRepository.Save(new PresetBuilder()
            .WithId(presetId)
            .WithOverclocking(() => new AmdGpuOverclockingBuilder().Build())
            .Build());


        // Act

        await _miningDeviceRepository.SetPreset(presetId, default, [.. deviceIds]);
        var checkingDevices = await _miningDeviceRepository
            .GetAvailableByPresetId(presetId, userId);


        // Assert

        Assert.That(checkingDevices, Is.Not.Null);
        Assert.That(checkingDevices, Has.Count.EqualTo(data.Count));
        for (var i = 0; i < data.Count; i++)
        {
            Assert.That(checkingDevices[i].PresetId, Is.EqualTo(presetId));
        }
    }

    [TestCaseSource(typeof(MiningDevicesTestCaseSource), nameof(MiningDevicesTestCaseSource.MiningDeviceListsWithVisiblePreset))]
    public async Task SetPreset_InvalidDeviceIds_ShouldNotSetPresetToDevices(List<MiningDeviceInfo> data)
    {
        // Arrange

        var deviceIds = data.Select(x => Guid.NewGuid());
        var presetId = Guid.NewGuid();
        var userId = MiningDevicesTestCaseSource.UserId;

        await PrepareDataBase(data);
        await _presetRepository.Save(new PresetBuilder()
            .WithId(presetId)
            .WithOverclocking(() => new AmdGpuOverclockingBuilder().Build())
            .Build());


        // Act

        await _miningDeviceRepository.SetPreset(presetId, default, [.. deviceIds]);
        var checkingDevices = await _miningDeviceRepository
            .GetAvailableByPresetId(presetId, userId);


        // Assert

        Assert.That(checkingDevices, Is.Not.Null);
        Assert.That(checkingDevices, Is.Empty);
    }

    [TestCaseSource(typeof(MiningDevicesTestCaseSource), nameof(MiningDevicesTestCaseSource.MiningDeviceListsWithVisiblePreset))]
    public async Task SetFlightSheet_ValidDeviceIdsAndFlightSheetId_ShouldSetFlightSheetOnDevices(List<MiningDeviceInfo> data)
    {
        // Arrange

        var deviceIds = data.Select(x => x.Id);
        var flightSheetId = Guid.NewGuid();

        await PrepareDataBase(data);


        // Act

        await _miningDeviceRepository.SetFlightSheet([.. deviceIds], flightSheetId);
        var checkingDevices = await _miningDeviceRepository
            .GetAvailable(new Specification(MiningDevicesTestCaseSource.UserId)).ToListAsync();


        // Assert

        Assert.That(checkingDevices, Is.Not.Null);
        Assert.That(checkingDevices, Has.Count.EqualTo(data.Count));
        for (var i = 0; i < data.Count; i++)
        {
            Assert.Multiple(() =>
            {
                Assert.That(checkingDevices[i].FlightSheetId, Is.EqualTo(flightSheetId));
                Assert.That(checkingDevices[i].FlightSheetConfirmationState, Is.EqualTo(FlightSheetConfirmationState.Unconfirmed));
            });
        }
    }

    [TestCaseSource(typeof(MiningDevicesTestCaseSource), nameof(MiningDevicesTestCaseSource.MiningDeviceListsWithVisiblePreset))]
    public async Task SetFlightSheet_InvalidDeviceIdsAndFlightSheetId_ShouldNotSetFlight(List<MiningDeviceInfo> data)
    {
        // Arrange

        var deviceIds = data.Select(x => Guid.NewGuid());
        var flightSheetId = Guid.NewGuid();

        await PrepareDataBase(data);


        // Act

        await _miningDeviceRepository.SetFlightSheet([.. deviceIds], flightSheetId);
        var checkingDevices = await _miningDeviceRepository
            .GetAvailable(new Specification(MiningDevicesTestCaseSource.UserId)).ToListAsync();


        // Assert

        Assert.That(checkingDevices, Is.Not.Null);
        for (var i = 0; i < data.Count; i++)
        {
            Assert.That(checkingDevices[i].FlightSheetId, Is.Not.EqualTo(flightSheetId));
        }
    }

    [TestCaseSource(typeof(MiningDevicesTestCaseSource), nameof(MiningDevicesTestCaseSource.MiningDeviceListsWithVisiblePreset))]
    public async Task RemoveFlightSheet_ValidDeviceIds_ShouldRemoveFlightSheetsFromDevices(List<MiningDeviceInfo> data)
    {
        // Arrange

        var deviceIds = data.Select(x => x.Id);

        await PrepareDataBase(data);


        // Act

        await _miningDeviceRepository.RemoveFlightSheet([.. deviceIds]);
        var checkingDevices = await _miningDeviceRepository
            .GetAvailable(new Specification(MiningDevicesTestCaseSource.UserId)).ToListAsync();


        // Assert

        Assert.That(checkingDevices, Is.Not.Null);
        Assert.That(checkingDevices, Has.Count.EqualTo(data.Count));
        for (var i = 0; i < data.Count; i++)
        {
            Assert.Multiple(() =>
            {
                Assert.That(data[i].FlightSheetId, Is.Not.Null);
                Assert.That(checkingDevices[i].FlightSheetId, Is.Null);
                Assert.That(checkingDevices[i].FlightSheetConfirmationState, Is.EqualTo(FlightSheetConfirmationState.Unconfirmed));
            });
        }
    }

    [TestCaseSource(typeof(MiningDevicesTestCaseSource), nameof(MiningDevicesTestCaseSource.MiningDeviceListsWithVisiblePreset))]
    public async Task ConfirmFlightSheet_ValidDeviceIds_ShouldConfirmFlightSheetStatuses(List<MiningDeviceInfo> data)
    {
        // Arrange

        var deviceIds = data.Select(x => x.Id);

        await PrepareDataBase(data);


        // Act

        await _miningDeviceRepository.ConfirmFlightSheet([.. deviceIds]);
        var checkingDevices = await _miningDeviceRepository
            .GetAvailable(new Specification(MiningDevicesTestCaseSource.UserId)).ToListAsync();


        // Assert

        Assert.That(checkingDevices, Is.Not.Null);
        Assert.That(checkingDevices, Has.Count.EqualTo(data.Count));
        for (var i = 0; i < data.Count; i++)
        {
            Assert.Multiple(() =>
            {
                Assert.That(data[i].FlightSheetConfirmationState, Is.Not.EqualTo(FlightSheetConfirmationState.Successfully));
                Assert.That(checkingDevices[i].FlightSheetConfirmationState, Is.EqualTo(FlightSheetConfirmationState.Successfully));
            });
        }
    }

    [TestCaseSource(typeof(MiningDevicesTestCaseSource), nameof(MiningDevicesTestCaseSource.MiningDeviceListsWithVisiblePreset))]
    public async Task SetFlightSheetConfirmationStateToError_ValidDeviceIds_ShouldSetConfirmationStateToError(List<MiningDeviceInfo> data)
    {
        // Arrange

        var deviceIds = data.Select(x => x.Id);

        await PrepareDataBase(data);


        // Act

        await _miningDeviceRepository.SetFlightSheetConfirmationStateToError([.. deviceIds]);
        var checkingDevices = await _miningDeviceRepository
            .GetAvailable(new Specification(MiningDevicesTestCaseSource.UserId)).ToListAsync();


        // Assert

        Assert.That(checkingDevices, Is.Not.Null);
        Assert.That(checkingDevices, Has.Count.EqualTo(data.Count));
        for (var i = 0; i < data.Count; i++)
        {
            Assert.Multiple(() =>
            {
                Assert.That(data[i].FlightSheetConfirmationState, Is.Not.EqualTo(FlightSheetConfirmationState.Error));
                Assert.That(checkingDevices[i].FlightSheetConfirmationState, Is.EqualTo(FlightSheetConfirmationState.Error));
            });
        }
    }
}
