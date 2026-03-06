using MNX.MonitoringCenter.Management.Core.Overclocking.Enums;
using MNX.MonitoringCenter.Management.Core.Overclocking.Gpu.Fan;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders.Overclockings;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.OuterModelBuilders.Overclockings;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Overclocking.Inventory.Gpu;
using MNX.MonitoringCenter.Management.UseCases.SetRigDevices;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Tests.Overclocking.Inventory;

using CoreNvidiaGpuOverclocking = Core.Overclocking.Gpu.NvidiaGpuOverclocking;
using InventoryNvidiaGpuOverclocking = MonitoringCenter.Inventory.Contracts.Devices.Gpu.Overclocking.NvidiaGpuOverclocking;

[TestFixture]
public sealed class NvidiaGpuOverclockingInventoryMapperTests
{
    private IOverclockingInventoryMapper<InventoryNvidiaGpuOverclocking,
                                         CoreNvidiaGpuOverclocking>
        _nvidiaGpuOverclockingInventoryMapper;

    [SetUp]
    public void SetUp()
    {
        _nvidiaGpuOverclockingInventoryMapper = new NvidiaGpuOverclockingInventoryMapper();
    }

    [Test]
    public void MapToCoreEntity()
    {
        // Arrange

        var inventoryGpuOverclocking = CreateInventoryNvidiaGpuOverclocking();


        // Act

        var mappedCoreGpuOverclocking = _nvidiaGpuOverclockingInventoryMapper.MapToCoreEntity(inventoryGpuOverclocking);


        // Assert

        Assert.That(mappedCoreGpuOverclocking, Is.Not.Null);
        Assert.That(mappedCoreGpuOverclocking, Is.TypeOf<CoreNvidiaGpuOverclocking>());
        Assert.Multiple(() =>
        {
            Assert.That(mappedCoreGpuOverclocking.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(mappedCoreGpuOverclocking.TargetDeviceType, Is.EqualTo(OverclockingTargetDeviceType.NvidiaGPU));

            var coreGpuOverclocking = (CoreNvidiaGpuOverclocking)mappedCoreGpuOverclocking;
            Assert.That(coreGpuOverclocking.PowerLimit, Is.EqualTo(inventoryGpuOverclocking.PowerLimit));
            Assert.That(coreGpuOverclocking.CoreClockLock, Is.EqualTo(inventoryGpuOverclocking.CoreClockLock));
            Assert.That(coreGpuOverclocking.CoreClockOffset, Is.EqualTo(inventoryGpuOverclocking.CoreClockOffset));
            Assert.That(coreGpuOverclocking.CoreVoltage, Is.EqualTo(inventoryGpuOverclocking.CoreVoltage));
            Assert.That(coreGpuOverclocking.CoreVoltageOffset, Is.EqualTo(inventoryGpuOverclocking.CoreVoltageOffset));
            Assert.That(coreGpuOverclocking.MemoryClockLock, Is.EqualTo(inventoryGpuOverclocking.MemoryClockLock));
            Assert.That(coreGpuOverclocking.MemoryClockOffset, Is.EqualTo(inventoryGpuOverclocking.MemoryClockOffset));
            Assert.That(coreGpuOverclocking.MemoryVoltage, Is.EqualTo(inventoryGpuOverclocking.MemoryVoltage));
            Assert.That(coreGpuOverclocking.MemoryVoltageOffset, Is.EqualTo(inventoryGpuOverclocking.MemoryVoltageOffset));

            Assert.That(coreGpuOverclocking.FanOverclocking, Is.Not.Null);
            Assert.That(coreGpuOverclocking.FanOverclocking.Type, Is.EqualTo(FanOverclockingType.TargetSpeed));
            Assert.That(coreGpuOverclocking.FanOverclocking, Is.TypeOf<FanOverclockingWithTargetSpeed>());

            var coreFanOverclocking = (FanOverclockingWithTargetSpeed)coreGpuOverclocking.FanOverclocking;
            Assert.That(coreFanOverclocking.TargetSpeed, Is.EqualTo(inventoryGpuOverclocking.FanSpeed));
        });
    }

    [Test]
    public void MapToModel()
    {
        // Arrange

        var coreGpuOverclocking = CreateCoreNvidiaGpuOverclocking();


        // Act

        var mappedInventoryGpuOverclocking = _nvidiaGpuOverclockingInventoryMapper.MapToModel(coreGpuOverclocking);


        // Assert

        Assert.That(mappedInventoryGpuOverclocking, Is.Not.Null);
        Assert.That(mappedInventoryGpuOverclocking, Is.TypeOf<InventoryNvidiaGpuOverclocking>());
        Assert.Multiple(() =>
        {
            var inventoryGpuOverclocking = (InventoryNvidiaGpuOverclocking)mappedInventoryGpuOverclocking;
            Assert.That(inventoryGpuOverclocking.PowerLimit, Is.EqualTo(coreGpuOverclocking.PowerLimit));
            Assert.That(inventoryGpuOverclocking.CoreClockLock, Is.EqualTo(coreGpuOverclocking.CoreClockLock));
            Assert.That(inventoryGpuOverclocking.CoreClockOffset, Is.EqualTo(coreGpuOverclocking.CoreClockOffset));
            Assert.That(inventoryGpuOverclocking.CoreVoltage, Is.EqualTo(coreGpuOverclocking.CoreVoltage));
            Assert.That(inventoryGpuOverclocking.CoreVoltageOffset, Is.EqualTo(coreGpuOverclocking.CoreVoltageOffset));
            Assert.That(inventoryGpuOverclocking.MemoryClockLock, Is.EqualTo(coreGpuOverclocking.MemoryClockLock));
            Assert.That(inventoryGpuOverclocking.MemoryClockOffset, Is.EqualTo(coreGpuOverclocking.MemoryClockOffset));
            Assert.That(inventoryGpuOverclocking.MemoryVoltage, Is.EqualTo(coreGpuOverclocking.MemoryVoltage));
            Assert.That(inventoryGpuOverclocking.MemoryVoltageOffset, Is.EqualTo(coreGpuOverclocking.MemoryVoltageOffset));
        });
    }

    private CoreNvidiaGpuOverclocking CreateCoreNvidiaGpuOverclocking()
    {
        return new NvidiaGpuOverclockingBuilder()
            .WithCoreClockLock(1950)
            .WithCoreClockOffset(120)
            .WithCoreVoltage(900)
            .WithCoreVoltageOffset(-50)
            .WithMemoryClockOffset(800)
            .WithPowerLimit(105)
            .Build();
    }

    private InventoryNvidiaGpuOverclocking CreateInventoryNvidiaGpuOverclocking()
    {
        return new InventoryNvidiaGpuOverclockingBuilder()
            .WithCoreClockLock(1950)
            .WithCoreClockOffset(120)
            .WithCoreVoltage(900)
            .WithCoreVoltageOffset(-50)
            .WithMemoryClockOffset(800)
            .WithFanSpeed(65)
            .WithPowerLimit(105)
            .Build();
    }
}
