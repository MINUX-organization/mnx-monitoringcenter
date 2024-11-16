using MNX.MonitoringCenter.Management.Core;
using System.Linq.Expressions;

namespace MNX.MonitoringCenter.Management.UseCases.Presets;

/// <summary>
/// Интерфейс репозитория для доступа к пресетам
/// </summary>
public interface IPresetRepository
{
    /// <summary>
    /// Получить пресет по идентификатору
    /// </summary>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <param name="id">  Уникальный идентификатор</param>
    /// <returns> Пресет </returns>
    Task<Preset?> GetAvailableById(Guid id, Guid userId);

    /// <summary>
    /// Получить список пресетов
    /// </summary>
    /// <param name="gpuName"> Название GPU </param>
    /// <param name="specification"> Спецификация. </param>
    /// <returns> Пресеты </returns>
    IAsyncEnumerable<Preset> GetAllAvailable(string? gpuName, Specification specification);

    /// <summary>
    /// Получить список пресетов сгруппированных по названию видеокарты.
    /// </summary>
    /// <param name="expression"> Выражение, указывающее правила группировки. </param>
    /// <param name="specification"> Спецификация. </param>
    /// <returns> Словарь, в котором ключ - название видеокарты, значение - список пресетов. </returns>
    Task<Dictionary<string, List<Preset>>> GetGroupedList(
        Expression<Func<Preset, string>> expression, Specification specification);

    /// <summary>
    /// Проверить наличие пресета по названию
    /// </summary>
    /// <param name="userId"> Идентификатор пользователя </param>
    /// <param name="name"> Название пресета ы</param>
    /// <returns> <see langword="true"/>, если пресет существует, иначе <see langword="false"/> </returns>
    Task<bool> Exists(Guid userId, string name);

    /// <summary>
    /// Сохранить пресет для выбранной 
    /// </summary>
    /// <param name="preset"> Пресет </param>
    Task Save(Preset preset);

    /// <summary>
    /// Обновить пресет
    /// </summary>
    /// <param name="preset"> Пресет </param>
    Task Update(Preset preset);

    /// <summary>
    /// Удалить пресет
    /// </summary>
    /// <param name="id"> Идентификатор пресета. </param>
    /// <param name="userId"> Идентификатор пользователя. </param>
    Task Remove(Guid id, Guid userId);
}