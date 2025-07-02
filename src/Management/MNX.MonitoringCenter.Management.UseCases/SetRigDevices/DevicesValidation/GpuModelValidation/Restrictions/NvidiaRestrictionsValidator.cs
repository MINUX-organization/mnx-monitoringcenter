using FluentValidation;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Restrictions;

namespace MNX.MonitoringCenter.Management.UseCases.SetRigDevices.DevicesValidation.GpuModelValidation.Restrictions;

/// <summary>
/// Валидатор параметров сущности <see cref="NvidiaGpuRestrictions"/>.
/// </summary>
public class NvidiaRestrictionsValidator : AbstractValidator<NvidiaGpuRestrictions>
{
    ///
    public NvidiaRestrictionsValidator()
    {
        this.ValidateRangeValues(x => x.ClockCoreLock);
        this.ValidateRangeValues(x => x.ClockCoreOffset);
        this.ValidateRangeValues(x => x.ClockMemoryLock);
        this.ValidateRangeValues(x => x.ClockMemoryOffset);
        this.ValidateRangeValues(x => x.VoltageCoreLock);
        this.ValidateRangeValues(x => x.VoltageCoreOffset);
        this.ValidateRangeValues(x => x.VoltageMemoryLock);
        this.ValidateRangeValues(x => x.VoltageMemoryOffset);
    }
}
