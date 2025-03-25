using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Commands.Models.MiningConfig;

/// <summary>
/// Базовая входная модель майнинг конфига.
/// </summary>
[JsonDerivedType(typeof(CpuMiningConfigInputModel), typeDiscriminator: "CPU")]
[JsonDerivedType(typeof(GpuMiningConfigInputModel), typeDiscriminator: "GPU")]

[SwaggerSubType(typeof(CpuMiningConfigInputModel), DiscriminatorValue = "CPU")]
[SwaggerSubType(typeof(GpuMiningConfigInputModel), DiscriminatorValue = "GPU")]
public abstract class MiningConfigInputModel
{
    /// <summary>
    /// Тип целевого майнинг устройства.
    /// </summary>
    public abstract MiningDeviceType DeviceType { get; }

    /// <summary>
    /// Список конфигов для майнинга монет.
    /// </summary>
    public List<MiningCoinConfigInputModel> CoinConfigs { get; init; } = new(0);

    /// <summary>
    /// Строка аргументов для майнера.
    /// </summary>
    public string? AdditionalArguments { get; init; }

    /// <summary>
    /// Строка конфигурации формата Json.
    /// </summary>
    public string? ConfigFileContent { get; init; }
}
