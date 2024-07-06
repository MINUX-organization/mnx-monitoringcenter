using MNX.MonitoringCenter.Monitoring.Core.Devices.Abstractions;
using MNX.MonitoringCenter.Monitoring.Core.Devices.Gpu;
using MNX.MonitoringCenter.Monitoring.UseCases.Queries.Devices;

namespace MNX.MonitoringCenter.Monitoring.UseCases.Abstractions;

/// <summary>
/// Репозиторий для майнинг устройств.
/// </summary>
public interface IMiningDeviceRepository
{
    /// <summary>
    /// Получить список майнинг устройств.
    /// </summary>
    /// <returns></returns>
    IAsyncEnumerable<MiningDevice> GetList(DeviceSpecification specification);

    /// <summary>
    /// Получить майнинг устройство по идентификатору.
    /// </summary>
    /// <param name="id"> Идентификатор. </param>
    /// <returns> Майнинг устройство. </returns>
    Task<MiningDevice?> GetById(Guid id);
}

/// <summary>
/// Репозиторий для разгоняемых майнинг устройств.
/// </summary>
/// <typeparam name="TOverclocking"> Тип разгона. </typeparam>
public interface IOverclockableMiningDeviceRepository<TOverclocking> where TOverclocking : IOverclocking
{
    /// <summary>
    /// Получить разгон по идентификатору майнинг устройства.
    /// </summary>
    /// <param name="id"> Идентификатор майнинга устройства. </param>
    /// <returns> Разгон видеокарты. </returns>
    Task<TOverclocking?> GetOverclocking(Guid id);

    /// <summary>
    /// Задать разгон майнинг устройству.
    /// </summary>
    /// <param name="id"> Идентификатор устройства. </param>
    /// <param name="overclocking"> Разгон. </param>
    Task SetOverclocking(Guid id, TOverclocking overclocking);
}

/// <summary>
/// Репозиторий для видеокарт.
/// </summary>
public interface IGpuRepository :
    IMiningDeviceRepository,
    IOverclockableMiningDeviceRepository<GpuOverclocking>
{

}
