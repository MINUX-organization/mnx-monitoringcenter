using MNX.MonitoringCenter.Management.DataAccess.FlightSheet.Dto.Target;

namespace MNX.MonitoringCenter.Management.DataAccess.FlightSheet.Dto;

/// <summary>
/// Dto полётного листа.
/// </summary>
internal class FlightSheetDto
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Название.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid OwnerId { get; set; }

    /// <summary>
    /// Таргеты.
    /// </summary>
    public List<BaseFlightSheetTargetDto> Targets { get; set; } = new(2);
}
