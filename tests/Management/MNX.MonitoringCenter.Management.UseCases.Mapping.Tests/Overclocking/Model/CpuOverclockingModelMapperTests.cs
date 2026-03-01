using MNX.MonitoringCenter.Management.Contracts.Overclocking.Cpu;
using MNX.MonitoringCenter.Management.Core.Overclocking.Cpu;
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

        Assert.Multiple(() =>
        {
            Assert.That(mappedOverclocking, Is.Not.Null);
            Assert.That(mappedOverclocking.TargetDeviceType, Is.EqualTo(cpuOverclockingModel.TargetDeviceType));
            Assert.That(mappedOverclocking, Is.TypeOf<CpuOverclocking>());

            var cpuOverclocking = (CpuOverclocking)mappedOverclocking;
            Assert.That(cpuOverclocking.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(cpuOverclocking.CoreClockLock, Is.EqualTo(cpuOverclockingModel.CoreClockLock));
            Assert.That(cpuOverclocking.CoreVoltage, Is.EqualTo(cpuOverclockingModel.CoreVoltage));
        });
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

        Assert.Multiple(() =>
        {
            Assert.That(mappedCpuOverclocking, Is.Not.Null);
            Assert.That(mappedCpuOverclocking.TargetDeviceType, Is.EqualTo(cpuOverclockingModel.TargetDeviceType));
            Assert.That(mappedCpuOverclocking, Is.TypeOf<CpuOverclocking>());

            var cpuOverclocking = (CpuOverclocking)mappedCpuOverclocking;
            Assert.That(cpuOverclocking.Id, Is.EqualTo(cpuOverclockingOriginal.Id));
            Assert.That(cpuOverclocking.CoreClockLock, Is.EqualTo(cpuOverclockingModel.CoreClockLock));
            Assert.That(cpuOverclocking.CoreVoltage, Is.EqualTo(cpuOverclockingModel.CoreVoltage));
        });
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

        Assert.Multiple(() =>
        {
            Assert.That(mappedCpuOverclockingModel, Is.Not.Null);
            Assert.That(mappedCpuOverclockingModel.TargetDeviceType, Is.EqualTo(cpuOverclocking.TargetDeviceType));
            Assert.That(mappedCpuOverclockingModel, Is.TypeOf<CpuOverclockingModel>());

            var cpuOverclockingModel = (CpuOverclockingModel)mappedCpuOverclockingModel;
            Assert.That(cpuOverclockingModel.CoreClockLock, Is.EqualTo(cpuOverclocking.CoreClockLock));
            Assert.That(cpuOverclockingModel.CoreVoltage, Is.EqualTo(cpuOverclocking.CoreVoltage));
        });
    }
}
