using MediatR;

namespace MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Gpu;

/// <summary>
/// Запрос на получение уникальных названий видеокарт.
/// </summary>
public sealed class GetGpuUniqueNamesQuery : IStreamRequest<string>
{
    /// <summary>
    /// Спецификация инвентаризации.
    /// </summary>
    public DeviceSpecification Specification { get; }

    public GetGpuUniqueNamesQuery(Guid userId)
    {
        Specification = new DeviceSpecification(userId);
    }
}
