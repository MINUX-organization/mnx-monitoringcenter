using MNX.MonitoringCenter.Management.Core;

namespace MNX.MonitoringCenter.Management.UseCases.Abstractions;

/// <summary>
/// Интерфейс репозитория для доступа к пресетам
/// </summary>
public interface IPresetRepository
{
    /// <summary>
    /// Получить пресет по идентификатору
    /// </summary>
    /// <param name="id">  Уникальный идентификатор</param>
    /// <returns> Пресет </returns>
    public Task<Preset?> GetById(Guid id);

    /// <summary>
    /// Получить список пресетов
    /// </summary>
    /// <param name="gpuName"> Название GPU </param>
    /// <returns> Пресеты </returns>
    public IAsyncEnumerable<Preset> GetPresets(string? gpuName);

    /// <summary>
    /// Сохранить пресет для выбранной 
    /// </summary>
    /// <param name="preset"> Пресет </param>
    public Task<Guid> Save(Preset preset);

    /// <summary>
    /// Обновить пресет
    /// </summary>
    /// <param name="preset"> Пресет </param>
    public Task Update(Preset preset);

    /// <summary>
    /// Удалить пресет
    /// </summary>
    /// <param name="preset"> Пресет </param>
    public Task Remove(Preset preset);
}