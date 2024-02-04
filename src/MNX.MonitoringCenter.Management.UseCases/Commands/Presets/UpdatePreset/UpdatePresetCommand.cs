using Kernel.UseCases;
using MediatR;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Presets.UpdatePreset;

/// <summary>
/// Команда редактировния пресета
/// </summary>
public class UpdatePresetCommand : IValidateableCommand<Unit>
{
    /// <summary>
    /// Уникальный идентиификатор
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Модель пресета
    /// </summary>
    public PresetModel Model { get; }

    public UpdatePresetCommand(Guid id, PresetModel model)
    {
        Id = id;
        Model = model;
    }
}
