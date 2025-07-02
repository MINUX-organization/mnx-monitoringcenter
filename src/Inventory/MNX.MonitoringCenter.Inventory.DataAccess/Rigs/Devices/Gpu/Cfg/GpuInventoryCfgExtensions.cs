using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu.Entities.Restrictions;
using System.Linq.Expressions;

namespace MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu.Cfg;

/// <summary>
/// Статический класс расширений конфигураций моделей видеокарт в базе данных.
/// </summary>
public static class GpuInventoryCfgExtensions
{
    /// <summary>
    /// Настроить маршрутизацию полей базового класса <see cref="GpuRestrictionsInventory"/>.
    /// </summary>
    public static void ConfigureBaseRestrictionsParams<TParent, TOwned>(
        this OwnedNavigationBuilder<TParent, TOwned> builder)
        where TOwned : GpuRestrictionsInventory
        where TParent : class
    {
        builder.ConfigureRestrictionsParams(
            (x => x.Power, "power"),
            (x => x.FanSpeed, "fan_speed"),
            (x => x.TemperatureCore, "temperature_core"),
            (x => x.TemperatureMemory, "temperature_memory")
        );
    }

    /// <summary>
    /// Настроить маршрутизацию полей класса <see cref="NvidiaGpuRestrictionsInventory"/>.
    /// </summary>
    public static void ConfigureNvidiaRestrictionsParams<TParent, TOwned>(
        this OwnedNavigationBuilder<TParent, TOwned> rest)
        where TOwned : NvidiaGpuRestrictionsInventory
        where TParent : class
    {
        rest.ConfigureRestrictionsParams(
            (x => x.ClockCoreLock, "nvidia_clock_core_lock"),
            (x => x.ClockCoreOffset, "nvidia_clock_core_offset"),
            (x => x.ClockMemoryLock, "nvidia_clock_memory_lock"),
            (x => x.ClockMemoryOffset, "nvidia_clock_memory_offset"),
            (x => x.VoltageCoreLock, "nvidia_voltage_core_lock"),
            (x => x.VoltageCoreOffset, "nvidia_voltage_core_offset"),
            (x => x.VoltageMemoryLock, "nvidia_voltage_memory_lock"),
            (x => x.VoltageMemoryOffset, "nvidia_voltage_memory_offset"));
    }

    /// <summary>
    /// Настроить маршрутизацию полей класса <see cref="AmdGpuRestrictionsInventory"/>.
    /// </summary>
    public static void ConfigureAmdRestrictionsParams<TParent, TOwned>(
        this OwnedNavigationBuilder<TParent, TOwned> rest)
        where TOwned : AmdGpuRestrictionsInventory
        where TParent : class
    {
        rest.ConfigureRestrictionsParams(
            (x => x.ClockCoreLock, "amd_clock_core_lock"),
            (x => x.ClockCoreState, "amd_clock_core_state"),
            (x => x.ClockMemoryLock, "amd_clock_memory_lock"),
            (x => x.ClockMemoryState, "amd_clock_memory_state"),
            (x => x.VoltageCoreLock, "amd_voltage_core_lock"),
            (x => x.VoltageCoreOffset, "amd_voltage_core_offset"),
            (x => x.VoltageMemoryLock, "amd_voltage_memory_lock"),
            (x => x.VoltageMemoryController, "amd_voltage_memory_controller"),
            (x => x.SocFrequency, "amd_soc_frequency"),
            (x => x.SocVoltage, "amd_soc_voltage"));
    }

    private static void ConfigureRestrictionsParams<TParent, TOwned>(
        this OwnedNavigationBuilder<TParent, TOwned> rest,
        params (Expression<Func<TOwned, GpuIntegerTypeRestrictionsInventory>> propertySelector, string prefix)[] props
    )
        where TOwned : GpuRestrictionsInventory
        where TParent : class
    {
        foreach (var (propertySelector, prefix) in props)
        {
            rest.OwnsOne(propertySelector!, value => value.ConfigureRestrictions(prefix));
        }
    }

    private static void ConfigureRestrictions<TOwner>(
        this OwnedNavigationBuilder<TOwner, GpuIntegerTypeRestrictionsInventory> builder,
        string prefix) where TOwner : class
    {
        builder.Property(x => x.Minimal).HasColumnName($"{prefix}_minimal");
        builder.Property(x => x.Maximal).HasColumnName($"{prefix}_maximal");
        builder.Property(x => x.Default).HasColumnName($"{prefix}_default");
        builder.Property(x => x.IsWritable).HasColumnName($"{prefix}_is_writable");
    }
}
