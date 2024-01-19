namespace MINUX.Backend.Worker.Core;

/// <summary>
/// Пресет
/// </summary>
public class Preset
{
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Тактовая чатсота памяти в мегагерцах
    /// </summary>
    public int MemoryClock { get; set; }

    /// <summary>
    /// Тактовая частота ядра в мегагерцах
    /// </summary>
    public int CoreClock { get; set; }

    /// <summary>
    /// Ограничение мощности в ваттах
    /// </summary>
    public int PowerLimit { get; set; }

    /// <summary>
    /// Критическая температура в градусах Цельсия
    /// </summary>
    public int CriticalTemperature { get; set; }

    /// <summary>
    /// Скорость вентилятора в процентах
    /// </summary>
    public int FanSpeed { get; set; }

    /// <summary>
    /// Название GPU
    /// </summary>
    public string GpuName { get; set; } = string.Empty;
}