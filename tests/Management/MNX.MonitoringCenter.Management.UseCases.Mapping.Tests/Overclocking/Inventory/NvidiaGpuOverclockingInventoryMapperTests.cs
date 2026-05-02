using MNX.MonitoringCenter.Management.Tests.Service.Assertions.Overclocking;
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
    public void MapToCoreEntity_ValidInventoryModel_ReturnsCoreEntity()
    {
        // Arrange

        var inventoryGpuOverclocking = CreateInventoryNvidiaGpuOverclocking();


        // Act

        var mappedCoreGpuOverclocking = _nvidiaGpuOverclockingInventoryMapper.MapToCoreEntity(inventoryGpuOverclocking);


        // Assert

        mappedCoreGpuOverclocking.ShouldBeEquivalentTo(inventoryGpuOverclocking);
    }

    [Test]
    public void MapToModel_ValidCoreEntity_ReturnsInventoryModel()
    {
        // Arrange

        var coreGpuOverclocking = CreateCoreNvidiaGpuOverclocking();


        // Act

        var mappedInventoryGpuOverclocking = _nvidiaGpuOverclockingInventoryMapper.MapToModel(coreGpuOverclocking);


        // Assert

        mappedInventoryGpuOverclocking.ShouldBeEquivalentTo(coreGpuOverclocking);
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
            .WithMemoryClockLock()
            .WithMemoryClockOffset(800)
            .WithMemoryVoltage()
            .WithMemoryVoltageOffset()
            .WithFanSpeed(65)
            .WithPowerLimit(105)
            .Build();
    }
}
