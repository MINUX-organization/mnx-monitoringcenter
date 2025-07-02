using FluentValidation;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Restrictions;

namespace MNX.MonitoringCenter.Inventory.UseCases.RigInventory.RigInventoryValidation.GpuModelValidation.Restrictions;

/// <summary>
/// Валидатор параметров сущности <see cref="GpuRestrictions"/>.
/// </summary>
public class GpuRestrictionsValidator : AbstractValidator<GpuRestrictions>
{
    ///
    public GpuRestrictionsValidator()
    {
        this.ValidateRangeValues(x => x.Power);
        this.ValidateRangeValues(x => x.FanSpeed);
        this.ValidateRangeValues(x => x.TemperatureCore);
        this.ValidateRangeValues(x => x.TemperatureMemory);
    }
}
