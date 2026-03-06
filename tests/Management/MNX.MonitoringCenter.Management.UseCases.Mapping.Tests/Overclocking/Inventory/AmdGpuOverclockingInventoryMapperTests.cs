using MNX.MonitoringCenter.Management.Core.Overclocking.Enums;
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

        var inventoryGpuOverclocking = CreateInventoryAmdGpuOverclocking();


        // Act

        var mappedCoreGpuOverclocking = _amdGpuOverclockingInventoryMapper.MapToCoreEntity(inventoryGpuOverclocking);


        // Assert

        Assert.That(mappedCoreGpuOverclocking, Is.Not.Null);
        Assert.That(mappedCoreGpuOverclocking, Is.TypeOf<CoreAmdGpuOverclocking>());
        Assert.Multiple(() =>
        {
            Assert.That(mappedCoreGpuOverclocking.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(mappedCoreGpuOverclocking.TargetDeviceType, Is.EqualTo(OverclockingTargetDeviceType.AmdGPU));

            var coreGpuOverclocking = (CoreAmdGpuOverclocking)mappedCoreGpuOverclocking;
            Assert.That(coreGpuOverclocking.CoreClockLock, Is.EqualTo(inventoryGpuOverclocking.CoreClockLock));
            Assert.That(coreGpuOverclocking.CoreClockState, Is.EqualTo(inventoryGpuOverclocking.CoreClockState));
            Assert.That(coreGpuOverclocking.CoreVoltage, Is.EqualTo(inventoryGpuOverclocking.CoreVoltage));
            Assert.That(coreGpuOverclocking.CoreClockState, Is.EqualTo(inventoryGpuOverclocking.CoreClockState));
            Assert.That(coreGpuOverclocking.MemoryClockLock, Is.EqualTo(inventoryGpuOverclocking.MemoryClockLock));
            Assert.That(coreGpuOverclocking.MemoryClockState, Is.EqualTo(inventoryGpuOverclocking.MemoryClockState));
            Assert.That(coreGpuOverclocking.MemoryVoltage, Is.EqualTo(inventoryGpuOverclocking.MemoryVoltage));
            Assert.That(coreGpuOverclocking.MemoryControllerVoltage, Is.EqualTo(inventoryGpuOverclocking.MemoryControllerVoltage));
            Assert.That(coreGpuOverclocking.MemoryTweak, Is.EqualTo(inventoryGpuOverclocking.MemoryTweak));
            Assert.That(coreGpuOverclocking.EnhancedOverclock, Is.EqualTo(inventoryGpuOverclocking.EnhancedOverclock));
            Assert.That(coreGpuOverclocking.SocFrequency, Is.EqualTo(inventoryGpuOverclocking.SocFrequency));
            Assert.That(coreGpuOverclocking.SocVoltage, Is.EqualTo(inventoryGpuOverclocking.SocVoltage));
            Assert.That(coreGpuOverclocking.PowerLimit, Is.EqualTo(inventoryGpuOverclocking.PowerLimit));
            Assert.That(coreGpuOverclocking.AlternativeDownVoltage, Is.EqualTo(inventoryGpuOverclocking.AlternativeDownVoltage));
        });
    }

    [Test]
    public void MapToModel_ValidCoreEntity_ReturnsInventoryModel()
    {
        // Arrange

        var coreGpuOverclocking = CreateCoreAmdGpuOverclocking();


        // Act

        var mappedInventoryGpuOverclocking = _amdGpuOverclockingInventoryMapper.MapToModel(coreGpuOverclocking);


        // Assert

        Assert.That(mappedInventoryGpuOverclocking, Is.Not.Null);
        Assert.That(mappedInventoryGpuOverclocking, Is.TypeOf<InventoryAmdGpuOverclocking>());
        Assert.Multiple(() =>
        {
            var inventoryGpuOverclocking = (InventoryAmdGpuOverclocking)mappedInventoryGpuOverclocking;
            Assert.That(inventoryGpuOverclocking.CoreClockLock, Is.EqualTo(coreGpuOverclocking.CoreClockLock)); ;
            Assert.That(inventoryGpuOverclocking.CoreClockState, Is.EqualTo(coreGpuOverclocking.CoreClockState));
            Assert.That(inventoryGpuOverclocking.CoreVoltage, Is.EqualTo(coreGpuOverclocking.CoreVoltage));
            Assert.That(inventoryGpuOverclocking.CoreClockState, Is.EqualTo(coreGpuOverclocking.CoreClockState));
            Assert.That(inventoryGpuOverclocking.MemoryClockLock, Is.EqualTo(coreGpuOverclocking.MemoryClockLock));
            Assert.That(inventoryGpuOverclocking.MemoryClockState, Is.EqualTo(coreGpuOverclocking.MemoryClockState));
            Assert.That(inventoryGpuOverclocking.MemoryVoltage, Is.EqualTo(coreGpuOverclocking.MemoryVoltage));
            Assert.That(inventoryGpuOverclocking.MemoryControllerVoltage, Is.EqualTo(coreGpuOverclocking.MemoryControllerVoltage));
            Assert.That(inventoryGpuOverclocking.MemoryTweak, Is.EqualTo(coreGpuOverclocking.MemoryTweak));
            Assert.That(inventoryGpuOverclocking.EnhancedOverclock, Is.EqualTo(coreGpuOverclocking.EnhancedOverclock));
            Assert.That(inventoryGpuOverclocking.SocFrequency, Is.EqualTo(coreGpuOverclocking.SocFrequency));
            Assert.That(inventoryGpuOverclocking.SocVoltage, Is.EqualTo(coreGpuOverclocking.SocVoltage));
            Assert.That(inventoryGpuOverclocking.PowerLimit, Is.EqualTo(coreGpuOverclocking.PowerLimit));
            Assert.That(inventoryGpuOverclocking.AlternativeDownVoltage, Is.EqualTo(coreGpuOverclocking.AlternativeDownVoltage));
        });
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
