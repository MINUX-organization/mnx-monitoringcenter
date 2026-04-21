using MNX.MonitoringCenter.Management.Contracts.Overclocking.Cpu;
using MNX.MonitoringCenter.Management.Core.Overclocking.Cpu;
using MNX.MonitoringCenter.Management.Tests.Service.Assertions.Overclocking;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.ContractBuilders.Overclockings;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders.Overclockings;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Overclocking.Model.Cpu;
using MNX.MonitoringCenter.Management.UseCases.Overclocking;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Tests.Overclocking.Model;

[TestFixture]
public sealed class CpuOverclockingModelMapperTests
{
    private IOverclockingModelMapper<CpuOverclockingModel, CpuOverclocking> _cpuOverclockingModelMapper;

    [SetUp]
    public void SetUp()
    {
        _cpuOverclockingModelMapper = new CpuOverclockingModelMapper();
    }

    [Test]
    public void MapToCoreEntity_ValidModel_ReturnsCpuOverclocking()
    {
        // Arrange

        var cpuOverclockingModel = new CpuOverclockingModelBuilder()
            .WithCoreClockLock(250)
            .WithCoreVoltage(350)
            .Build();


        // Act

        var mappedOverclocking = _cpuOverclockingModelMapper.MapToCoreEntity(cpuOverclockingModel);


        // Assert

        mappedOverclocking.ShouldBeEquivalentTo(cpuOverclockingModel);
    }

    [Test]
    public void MapToCoreEntity_ValidModelAndOriginalEntity_ReturnsCpuOverclocking()
    {
        // Arrange

        var cpuOverclockingModel = new CpuOverclockingModelBuilder()
            .WithCoreClockLock(250)
            .WithCoreVoltage(350)
            .Build();

        var cpuOverclockingOriginal = new CpuOverclockingBuilder()
            .WithCoreClockLock(250)
            .WithCoreVoltage(350)
            .Build();


        // Act

        var mappedCpuOverclocking = _cpuOverclockingModelMapper.MapToCoreEntity(cpuOverclockingModel, cpuOverclockingOriginal);


        // Assert

        mappedCpuOverclocking.ShouldBeEquivalentTo(cpuOverclockingModel, cpuOverclockingOriginal.Id);
    }

    [Test]
    public void MapToModel_ValidCpuOverclocking_ReturnsCpuOverclockingModel()
    {
        // Arrange

        var cpuOverclocking = new CpuOverclockingBuilder()
            .WithCoreClockLock(250)
            .WithCoreVoltage(350)
            .Build();


        // Act

        var mappedCpuOverclockingModel = _cpuOverclockingModelMapper.MapToModel(cpuOverclocking);


        // Assert

        mappedCpuOverclockingModel.ShouldBeEquivalentTo(cpuOverclocking);
    }
}
