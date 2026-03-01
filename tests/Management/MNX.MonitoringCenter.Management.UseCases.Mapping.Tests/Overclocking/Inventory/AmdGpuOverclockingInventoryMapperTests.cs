using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders.Overclockings;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.OuterModelBuilders.Overclockings;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Overclocking.Inventory.Gpu;
using MNX.MonitoringCenter.Management.UseCases.SetRigDevices;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Tests.Overclocking.Inventory;

using CoreAmdGpuOverclocking = Core.Overclocking.Gpu.AmdGpuOverclocking;
using InventoryAmdGpuOverclocking = MonitoringCenter.Inventory.Contracts.Devices.Gpu.Overclocking.AmdGpuOverclocking;

public sealed class AmdGpuOverclockingInventoryMapperTests
{
    private IOverclockingInventoryMapper<InventoryAmdGpuOverclocking,
                                         CoreAmdGpuOverclocking>
        _amdGpuOverclockingInventoryMapper;

    [SetUp]
    public void SetUp()
    {
        _amdGpuOverclockingInventoryMapper = new AmdGpuOverclockingInventoryMapper();
    }

    [Test]
    public void MapToCoreEntity_ValidInventoryModel_ReturnsCoreEntity()
    {
        // Arrange

        var inventoryGpuOvercloclocking = CreateInventoryAmdGpuOverclocking();


        // Act

        var mappedCoreGpuOverclocking = _amdGpuOverclockingInventoryMapper.MapToCoreEntity(inventoryGpuOvercloclocking);


        // Assert


    }

    [Test]
    public void MapToModel_ValidCoreEntity_ReturnsInventoryModel()
    {
        // Arrange




        // Act




        // Assert


    }

    private InventoryAmdGpuOverclocking CreateInventoryAmdGpuOverclocking()
    {
        return new InventoryAmdGpuOverclockingBuilder()
            .WithCoreClockLock(2650)
            .WithCoreClockState(1)
            .WithCoreVoltage(1125)
            .WithCoreVoltageOffset(-25)
            .WithMemoryClockLock(2100)
            .WithMemoryClockState(1)
            .WithMemoryVoltage(1350)
            .WithMemoryControllerVoltage(1000)
            .WithMemoryTweak("Fast")
            .WithPowerLimit(10)
            .WithFanSpeed(65)
            .WithAlternativeDownVoltage(false)
            .WithEnhancedOverclock(true)
            .WithSocFrequency(1200)
            .WithSocVoltage(1050)
            .Build();
    }

    private CoreAmdGpuOverclocking CreateCoreAmdGpuOverclocking()
    {
        return new AmdGpuOverclockingBuilder()
            .WithCoreClockLock(2650)
            .WithCoreClockState(1)
            .WithCoreVoltage(1125)
            .WithCoreVoltageOffset(-25)
            .WithMemoryClockLock(2100)
            .WithMemoryClockState(1)
            .WithMemoryVoltage(1350)
            .WithMemoryControllerVoltage(1000)
            .WithMemoryTweak("Fast")
            .WithPowerLimit(10)
            .WithAlternativeDownVoltage(false)
            .WithEnhancedOverclock(true)
            .WithSocFrequency(1200)
            .WithSocVoltage(1050)
            .Build();
    }
}
