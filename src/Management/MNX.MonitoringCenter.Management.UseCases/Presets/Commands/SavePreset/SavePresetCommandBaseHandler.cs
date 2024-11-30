using AutoMapper;
using MediatR;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Gpu;
using MNX.MonitoringCenter.Management.Contracts.Presets;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets;

namespace MNX.MonitoringCenter.Management.UseCases.Presets.Commands.SavePreset;

/// <summary>
/// Базовый обработчик команды сохранения пресета.
/// </summary>
public abstract class SavePresetCommandBaseHandler
{
    protected readonly IMapper _mapper;

    protected readonly IMediator _mediator;

    protected readonly IPresetRepository _repository;

    public SavePresetCommandBaseHandler(IMapper mapper,
                                        IMediator mediator,
                                        IPresetRepository presetRepository)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _repository = presetRepository ?? throw new ArgumentNullException(nameof(presetRepository));
    }

    /// <summary>
    /// Получить признак валидности разгона.
    /// </summary>
    /// <param name="gpuName"> Название видеокарты. </param>
    /// <param name="overclocking"> Разгон. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns> Результат валидации. </returns>
    protected async Task<Result<PresetModel>> IsValidOverclocking(string gpuName,
                                                                  OverclockingInputModel overclocking,
                                                                  CancellationToken cancellationToken)
    {
        var restrictions = await _mediator.Send(new GetGpuRestrictionsQuery(gpuName), cancellationToken);
        var validator = new OverclockingInputModelValidator(restrictions.GetValue());

        var validationResult = await validator.ValidateAsync(overclocking, cancellationToken);

        return validationResult.IsValid
                ? Result<PresetModel>.Empty()
                : Result<PresetModel>.Invalid(validationResult.Errors.Select(x => x.ErrorMessage).ToArray());
    }
}
