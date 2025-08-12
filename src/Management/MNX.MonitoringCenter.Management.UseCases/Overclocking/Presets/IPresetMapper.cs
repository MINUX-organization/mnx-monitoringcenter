using MNX.MonitoringCenter.Management.Contracts.Presets;
using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.UseCases.Overclocking.Presets.Commands;

namespace MNX.MonitoringCenter.Management.UseCases.Overclocking.Presets;

/// <summary>
/// Интерфейс маппера сущности <see cref="Preset"/> его моделей.
/// </summary>
public interface IPresetMapper
{
    /// <summary>
    /// Преобразовать <see cref="PresetInputModel"/> в новый <see cref="Preset"/>.
    /// </summary>
    /// <param name="new"> Модель данных. </param>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <returns> Новый <see cref="Preset"/>. </returns>
    Preset MapToCoreEntity(PresetInputModel @new, Guid userId);

    /// <summary>
    /// Преобразовать данные <see cref="PresetInputModel"/>
    /// в новый <see cref="Preset"/>, с использованием оригинальных immutable-полей.
    /// </summary>
    /// <param name="new"> Модель данных. </param>
    /// <param name="original"> Оригинальная модель. </param>
    /// <returns> Новый <see cref="Preset"/>. </returns>
    Preset MapToCoreEntity(PresetInputModel @new, Preset original);

    /// <summary>
    /// Преобразовать коллекцию <see cref="Preset"/> в коллекцию <see cref="PresetModel"/>.
    /// </summary>
    /// <param name="newPresets"> Коллекция экземпляров <see cref="Preset"/>. </param>
    /// <returns> Коллекция <see cref="PresetModel"/>. </returns>
    List<PresetModel> MapToCoreEntitiesList(List<Preset> newPresets);

    /// <summary>
    /// Преобразовать <see cref="Preset"/> в <see cref="PresetModel"/>.
    /// </summary>
    /// <param name="new"> Модель данных .</param>
    /// <returns> Новый <see cref="PresetModel"/>. </returns>
    PresetModel MapToModel(Preset @new);
}
