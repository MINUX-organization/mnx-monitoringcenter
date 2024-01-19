using Kernel.UseCases;
using MediatR;

namespace MINUX.Backend.Worker.UseCases.Commands.SavePresetCommand;

/// <summary>
/// Команда сохранение пресета для выбранной серии GPU
/// </summary>
public class SavePresetCommand : IRequest<Result<Unit>>
{
    /// <summary>
    /// Название GPU
    /// </summary>
    public string GpuName { get; }

    /// <summary>
    /// Тактовая чатсота памяти в мегагерцах
    /// </summary>
    public int MemoryClock { get; }

    /// <summary>
    /// Тактовая частота ядра в мегагерцах
    /// </summary>
    public int CoreClock { get; }

    /// <summary>
    /// Ограничение мощности в ваттах
    /// </summary>
    public int PowerLimit { get; }

    /// <summary>
    /// Критическая температура в градусах Цельсия
    /// </summary>
    public int CriticalTemperature { get; }

    /// <summary>
    /// Скорость вентилятора в процентах
    /// </summary>
    public int FanSpeed { get; }

    public SavePresetCommand(string gpuName, int memoryClock, int coreClock, int powerLimit, int criticalTemperature, int fanSpeed)
    {
        GpuName = gpuName;
        MemoryClock = memoryClock;
        CoreClock = coreClock;
        PowerLimit = powerLimit;
        CriticalTemperature = criticalTemperature;
        FanSpeed = fanSpeed;
    }
}
