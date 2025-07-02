using FluentValidation;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Restrictions;

namespace MNX.MonitoringCenter.Management.UseCases.SetRigDevices.DevicesValidation.GpuModelValidation.Restrictions;

/// <summary>
/// Валидатор параметров сущности <see cref="AmdGpuRestrictions"/>.
/// </summary>
public class AmdRestrictionsValidator : AbstractValidator<AmdGpuRestrictions>
{
    ///
    public AmdRestrictionsValidator()
    {
        this.ValidateRangeValues(x => x.ClockCoreLock);
        this.ValidateRangeValues(x => x.ClockCoreState);
        this.ValidateRangeValues(x => x.ClockMemoryLock);
        this.ValidateRangeValues(x => x.ClockMemoryState);
        this.ValidateRangeValues(x => x.VoltageCoreLock);
        this.ValidateRangeValues(x => x.VoltageCoreOffset);
        this.ValidateRangeValues(x => x.VoltageMemoryLock);
        this.ValidateRangeValues(x => x.VoltageMemoryController);
        this.ValidateRangeValues(x => x.SocFrequency);
        this.ValidateRangeValues(x => x.SocVoltage);
    }
}
