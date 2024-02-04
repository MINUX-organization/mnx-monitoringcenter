using MNX.MonitoringCenter.Management.Core.HardwareParameters;
using MNX.MonitoringCenter.Management.Core.HardwareParameters.Cpu;
using MNX.MonitoringCenter.Management.Core.HardwareParameters.Gpu;
using MNX.MonitoringCenter.Management.Core.HardwareParameters.Harddrive;
using MNX.MonitoringCenter.Management.Core.HardwareParameters.Motherboard;

namespace MNX.MonitoringCenter.Management.UseCases.Abstractions;

/// <summary>
/// Интерфес репозитория, предоставляющего методы для доступа к параметрам аппаратного обеспечения.
/// </summary>
public interface IHardwareParametersRepository
{
    /// <summary>
    /// Получить параметры для всех CPU
    /// </summary>
    /// <returns> Параметры для всех CPU </returns>
    Task<List<Cpu>> GetCpusParameters();

    /// <summary>
    /// Получить параметры для всех GPU
    /// </summary>
    /// <returns> Параметры для всех GPU </returns>
    Task<List<Gpu>> GetGpusParameters();

    /// <summary>
    /// Получить параметры всех дисков
    /// </summary>
    /// <returns> Параметры для всех дисков </returns>
    Task<List<Harddrive>> GetHarddrivesParameters();

    /// <summary>
    /// Получить параметры оперативной памяти
    /// </summary>
    /// <returns> Параметры оперативной памяти </returns>
    Task<List<Ram>> GetRamsParameters();

    /// <summary>
    /// Получить парамметры материнской платы
    /// </summary>
    /// <returns> Параметры материнской платы </returns>
    Task<Motherboard> GetMotherboardParameters();

    /// <summary>
    /// Получить информацию о системе
    /// </summary>
    /// <returns> Информация о системе </returns>
    Task<SystemInfo> GetSystemInfo();
}
