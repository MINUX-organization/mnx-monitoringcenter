using AutoMapper;
using MediatR;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Gpu;
using MNX.MonitoringCenter.Management.Contracts.Presets;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Presets;
using MNX.MonitoringCenter.Management.UseCases.Presets.Commands.SavePreset;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Presets.SavePreset;

/// <summary>
/// Обработчик команды сохранения пресета для выбранной серии GPU
/// </summary>
public class SavePresetCommandHandler :
    SavePresetCommandBaseHandler,
    IRequestHandler<SavePresetCommand, Result<PresetModel>>
{
    public SavePresetCommandHandler(IMapper mapper,
                                    IMediator mediator,
                                    IPresetRepository repository)
        : base(mapper, mediator, repository) { }

    public async Task<Result<PresetModel>> Handle(SavePresetCommand request, CancellationToken cancellationToken)
    {
        if (await _repository.Exists(request.UserId, request.Model.Name, cancellationToken))
        {
            return Result<PresetModel>.Conflict("Preset already exists");
        }

        if (!await IsValidName(request.UserId, request.Model.GpuName, cancellationToken))
        {
            return Result<PresetModel>.Invalid("Invalid GPU name.");
        }

        var overclockingValidationResult = await IsValidOverclocking(request.Model.GpuName!,
                                                                     request.Model.Overclocking,
                                                                     cancellationToken);
        if (!overclockingValidationResult.IsSuccess)
        {
            return overclockingValidationResult;
        }

        var preset = _mapper.Map<Preset>(request);
        await _repository.Save(preset);

        return Result<PresetModel>.SuccessfullyCreated(_mapper.Map<PresetModel>(preset));
    }

    /// <summary>
    /// Получить признак валидности названия видеокарты.
    /// </summary>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <param name="gpuName"> Название видеокарты. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns> Результат проверки. </returns>
    private async Task<bool> IsValidName(Guid userId, string? gpuName, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(gpuName))
        {
            return true;
        }

        var gpuNamesStream = _mediator.CreateStream(new GetGpuUniqueNamesQuery(userId), cancellationToken);

        await foreach (var name in gpuNamesStream.WithCancellation(cancellationToken))
        {
            if (string.Equals(name, gpuName, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }
}
