using MINUX.Backend.Worker.Core.HardwareParameters;
using MINUX.Backend.Worker.Core.HardwareParameters.Cpu;
using MINUX.Backend.Worker.Core.HardwareParameters.Gpu;
using MINUX.Backend.Worker.Core.HardwareParameters.Harddrive;
using MINUX.Backend.Worker.Core.HardwareParameters.Motherboard;

namespace MINUX.Backend.Worker.UseCases.Abstractions;

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
    /// Получить парамметры материнской платы
    /// </summary>
    /// <returns> Параметры материнской платы </returns>
    Task<Motherboard> GetMotherboardParameters();

    /// <summary>
    /// Получить параметры оперативной памяти
    /// </summary>
    /// <returns> Параметры оперативной памяти </returns>
    Task<Ram> GetRamParameters();

    /// <summary>
    /// Получить информацию о системе
    /// </summary>
    /// <returns> Информация о системе </returns>
    Task<SystemInfo> GetSystemInfo();
}
