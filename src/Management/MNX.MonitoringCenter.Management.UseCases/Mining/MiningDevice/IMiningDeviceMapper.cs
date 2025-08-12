using MNX.MonitoringCenter.Management.Contracts.MiningDevice;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.MiningDevice;

/// <summary>
/// Интерфейс маппера сущности <see cref="MiningDeviceInfo"/> и её моделей.
/// </summary>
public interface IMiningDeviceMapper
{
    /// <summary>
    /// Преобразовать сущность <see cref="MiningDeviceInfo"/> в <see cref="MiningDeviceModel"/>.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    MiningDeviceModel MapToModel(MiningDeviceInfo model);
}
