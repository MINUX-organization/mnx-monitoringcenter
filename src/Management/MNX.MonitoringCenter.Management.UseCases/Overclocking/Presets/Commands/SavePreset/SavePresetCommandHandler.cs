using MediatR;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Management.Contracts.Presets;
using MNX.MonitoringCenter.Management.UseCases.Mining.MiningDevice;

namespace MNX.MonitoringCenter.Management.UseCases.Overclocking.Presets.Commands.SavePreset;

/// <summary>
/// Обработчик команды сохранения пресета для выбранной серии GPU
/// </summary>
public class SavePresetCommandHandler :
    SaveOverclockingBaseHandler,
    IRequestHandler<SavePresetCommand, Result<PresetModel>>
{
    private readonly IPresetMapper _presetMapper;

    private readonly IPresetRepository _presetRepository;

    private readonly IMiningDeviceRepository _miningDeviceRepository;

    ///
    public SavePresetCommandHandler(IMediator mediator,
                                    IPresetMapper presetMapper,
                                    IPresetRepository presetRepository,
                                    IMiningDeviceRepository miningDeviceRepository)
        : base(mediator)
    {
        _presetMapper = presetMapper ??
            throw new ArgumentNullException(nameof(presetMapper));

        _presetRepository = presetRepository
            ?? throw new ArgumentNullException(nameof(presetRepository));

        _miningDeviceRepository = miningDeviceRepository
            ?? throw new ArgumentNullException(nameof(miningDeviceRepository));
    }

    ///
    public async Task<Result<PresetModel>> Handle(SavePresetCommand request, CancellationToken cancellationToken)
    {
        if (await _presetRepository.Exists(request.UserId, request.Model.Name, cancellationToken))
        {
            return Result<PresetModel>.Conflict("Preset already exists");
        }

        if (!await _miningDeviceRepository.Exists(request.Model.DeviceName!, request.UserId, cancellationToken))
        {
            return Result<PresetModel>.Invalid("Invalid mining device name.");
        }

        var preset = _presetMapper.MapToCoreEntity(request.Model, request.UserId);

        var overclockingValidationResult = await ValidateOverclocking(
            request.Model.DeviceName!, preset.Overclocking!, cancellationToken);

        if (!overclockingValidationResult.IsSuccess)
        {
            return Result<PresetModel>.Invalid(overclockingValidationResult.Errors ?? new string[] { });
        }

        await _presetRepository.Save(preset);

        return Result<PresetModel>.SuccessfullyCreated(_presetMapper.MapToModel(preset));
    }
}
