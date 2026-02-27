using MNX.MonitoringCenter.Management.Tests.Service.Builders.ContractBuilders.Overclockings;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.ContractBuilders.Overclockings.Fan;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Tests.Overclocking;

[TestFixture]
public class NvidiaGpuOverclockingModelMapperTests
{
    [SetUp]
    public void SetUp()
    {

    }

    [Test]
    public void MapToCoreEntity_ValidModel_ReturnsNvidiaGpuOverclocking()
    {
        // Arrange

        var nvidiaCpuOverclockingModel = new NvidiaGpuOverclockingModelBuilder()
            .WithFanOverclocking(() =>
            {
                return new FanOverclockingWithTargetSpeedModelBuilder()
                    .WithTargetSpeed(100)
                    .Build();
            })
            .WithCoreClockOffset(120)
            .WithCoreVoltageOffset(-50)
            .WithMemoryClockOffset(700)
            .WithPowerLimit(105)
            .Build();


        // Act




        // Assert


    }

    [Test]
    public void MapToCoreEntity_ValidModelAndOriginal_ReturnsNvidiaGpuOverclocking()
    {
        // Arrange




        // Act




        // Assert


    }

    [Test]
    public void MapToModel_ValidCoreEntity_ReturnsNvidiaGpuOverclockingModel()
    {
        // Arrange




        // Act




        // Assert


    }
}
