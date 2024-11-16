using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace MNX.MonitoringCenter.Management.Contracts.FlightSheet.MiningConfigs;

/// <summary>
/// Базовая модель майнинга конфига.
/// </summary>
[JsonDerivedType(typeof(CpuMiningConfigModel), typeDiscriminator: "CPU")]
[JsonDerivedType(typeof(GpuMiningConfigModel), typeDiscriminator: "GPU")]

[SwaggerSubType(typeof(CpuMiningConfigModel), DiscriminatorValue = "CPU")]
[SwaggerSubType(typeof(GpuMiningConfigModel), DiscriminatorValue = "GPU")]
public abstract class BaseMiningConfigModel
{
    /// <summary>
    /// Список конфигов для майнинга.
    /// </summary>
    public List<MiningCoinConfigModel> CoinConfigs { get; set; } = new(0);

    /// <summary>
    /// Строка аргументов для майнера.
    /// </summary>
    public string? AdditionalArguments { get; set; }

    /// <summary>
    /// Строка конфигурации формата Json.
    /// </summary>
    public string? ConfigFileContent { get; set; }
}
