using MNX.MonitoringCenter.Management.Core.Overclocking.Cpu;
using MNX.MonitoringCenter.Management.Tests.Service.Assertions.Overclocking;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders.Overclockings;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.OuterModelBuilders.Overclockings;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Overclocking.Inventory.Cpu;
using MNX.MonitoringCenter.Management.UseCases.SetRigDevices;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Tests.Overclocking.Inventory;

using InventoryCpuOverclocking = MonitoringCenter.Inventory.Contracts.Devices.Cpu.CpuOverclocking;

[TestFixture]
public sealed class CpuOverclockingInventoryMapperTests
{
    private IOverclockingInventoryMapper<InventoryCpuOverclocking, CpuOverclocking> _cpuOverclockingInventoryMapper;

    [SetUp]
    public void SetUp()
    {
        _cpuOverclockingInventoryMapper = new CpuOverclockingInventoryMapper();
    }

    [Test]
    public void MapToCoreEntity_ValidInventoryCpuModel_ReturnsCoreEntity()
    {
        // Arrange

        var inventoryCpuOverclocking = new InventoryCpuOverclockingBuilder()
            .WithCoreClockLock(100)
            .WithCoreVoltage(50)
            .Build();


        // Act

        var mappedCoreCpuOverclocking = _cpuOverclockingInventoryMapper.MapToCoreEntity(inventoryCpuOverclocking);


        // Assert

        mappedCoreCpuOverclocking.ShouldBeEquivalentTo(inventoryCpuOverclocking);
    }

    [Test]
    public void MapToModel_ValidCoreEntity_ReturnsInventoryCpuModel()
    {
        // Arrange

        var coreCpuOverclocking = new CpuOverclockingBuilder()
            .WithCoreClockLock(100)
            .WithCoreVoltage(50)
            .Build();


        // Act

        var mappedInventoryCpuOverclocking = _cpuOverclockingInventoryMapper.MapToModel(coreCpuOverclocking);


        // Assert

        mappedInventoryCpuOverclocking.ShouldBeEquivalentTo(coreCpuOverclocking);
    }
}
