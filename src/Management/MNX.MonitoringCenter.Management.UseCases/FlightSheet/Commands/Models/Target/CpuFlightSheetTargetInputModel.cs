using MNX.MonitoringCenter.Management.Core.FlightSheet.Target;

namespace MNX.MonitoringCenter.Management.UseCases.FlightSheet.Commands.Models.Target;

/// <summary>
/// Входная модель таргета полётного листа для процессора.
/// </summary>
public class CpuFlightSheetTargetInputModel : FlightSheetTargetInputModel
{
    /// <inheritdoc/>
    public override FlightSheetTargetType Type
    {
        get => FlightSheetTargetType.CPU;
    }

    /// <summary>
    /// Страницы.
    /// </summary>
    public int HugePage { get; init; }

    /// <summary>
    /// Строка конфигурации формата Json.
    /// </summary>
    public string? ConfigFile { get; init; }
}
