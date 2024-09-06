using MNX.MonitoringCenter.Monitoring.Core.Devices.Enums;

namespace MNX.MonitoringCenter.Monitoring.UseCases.Queries.Devices;

/// <summary>
/// Спецификация для майнинг устройств.
/// </summary>
public class DeviceSpecification : Specification
{
    /// <summary>
    /// Типы майнинг устройства.
    /// </summary>
    public List<MiningDeviceType> Types { get; } = new();

    public DeviceSpecification(Guid userId,
                                     List<MiningDeviceType> types,
                                     string? searchString = null,
                                     string? filterString = null,
                                     string[]? filterArguments = null)
        : base(userId, searchString, filterString, filterArguments)
    {
        Types = types;
    }
}
