using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets;

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
    public Task<Preset?> GetAvailableById(Guid id, long userId);

    /// <summary>
    /// Получить список пресетов
    /// </summary>
    /// <param name="gpuName"> Название GPU </param>
    /// <returns> Пресеты </returns>
    public IAsyncEnumerable<Preset> GetAllAvailable(string? gpuName, long userId);

    /// <summary>
    /// Проверить наличие пресета по названию
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <param name="name">Название пресета</param>
    /// <returns></returns>
    public Task<bool> Exists(long userId, string name);

    /// <summary>
    /// Проверить наличие пресета по названию, не учитывая текущий пресет
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <param name="name">Название пресета</param>
    /// <param name="gpuName">Название видеокарты</param>
    /// <returns></returns>
    public Task<bool> Exists(long userId, string name, string gpuName, Guid Id);

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