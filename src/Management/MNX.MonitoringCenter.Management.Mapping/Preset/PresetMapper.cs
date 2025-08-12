using MNX.MonitoringCenter.Management.Contracts.Presets;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Infrastructure.Overclocking.Model;
using MNX.MonitoringCenter.Management.UseCases.Overclocking.Presets;
using MNX.MonitoringCenter.Management.UseCases.Overclocking.Presets.Commands;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Preset;

/// <summary>
/// Реализация <see cref="IPresetMapper"/>.
/// </summary>
public class PresetMapper : IPresetMapper
{
    private readonly OverclockingModelMapperRegistry _registry;

    ///
    public PresetMapper(OverclockingModelMapperRegistry registry)
    {
        _registry = registry ?? throw new ArgumentNullException(nameof(registry));
    }

    /// <inheritdoc/>
    public Core.Overclocking.Preset MapToCoreEntity(PresetInputModel @new, Guid ownerId)
    {
        var overclocking = _registry.MapToCoreEntity(@new.Overclocking);
        return new Core.Overclocking.Preset()
        {
            Name = @new.Name,
            DeviceName = @new.DeviceName,
            Overclocking = overclocking,
            OverclockingId = overclocking.Id,
            OwnerId = ownerId,
            IsVisible = true
        };
    }

    /// <inheritdoc/>
    public Core.Overclocking.Preset MapToCoreEntity(PresetInputModel @new, Core.Overclocking.Preset original)
    {
        var overclocking = _registry.MapToCoreEntity(@new.Overclocking, original.Overclocking);
        return new Core.Overclocking.Preset()
        {
            Id = original.Id,
            Name = @new.Name,
            DeviceName = original.DeviceName,
            Overclocking = overclocking,
            OverclockingId = original.OverclockingId,
            OwnerId = original.OwnerId,
            IsVisible = original.IsVisible,
        };
    }

    /// <inheritdoc/>
    public List<PresetModel> MapToCoreEntitiesList(List<Core.Overclocking.Preset> newPresets)
    {
        var result = new List<PresetModel>();

        foreach (var preset in newPresets)
        {
            var overclocking = _registry.MapToModel(preset.Overclocking);
            var model = new PresetModel()
            {
                Id = preset.Id,
                Name = preset.Name,
                DeviceName = preset.DeviceName,
                Overclocking = overclocking
            };
            result.Add(model);
        }

        return result;
    }

    /// <inheritdoc/>
    public PresetModel MapToModel(Core.Overclocking.Preset @new)
    {
        var overclocking = _registry.MapToModel(@new.Overclocking);
        return new PresetModel()
        {
            Id = @new.Id,
            Name = @new.Name,
            DeviceName = @new.DeviceName,
            Overclocking = overclocking
        };
    }
}
