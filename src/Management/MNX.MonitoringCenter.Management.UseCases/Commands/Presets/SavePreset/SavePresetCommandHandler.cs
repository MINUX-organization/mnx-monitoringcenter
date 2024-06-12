using AutoMapper;
using MediatR;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Presets.SavePreset;

/// <summary>
/// Обработчик команды сохранения пресета для выбранной серии GPU
/// </summary>
public class SavePresetCommandHandler : IRequestHandler<SavePresetCommand, Result<PresetModel>>
{
    private readonly IPresetRepository _presetRepository;

    private readonly IMonitoringClient _monitoringClient;

    private readonly IMapper _mapper;

    public SavePresetCommandHandler(IPresetRepository repository,
                                    IMonitoringClient monitoringClient,
                                    IMapper mapper)
    {
        _presetRepository = repository ?? throw new ArgumentNullException(nameof(repository));
        _monitoringClient = monitoringClient ?? throw new ArgumentNullException(nameof(monitoringClient));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Result<PresetModel>> Handle(SavePresetCommand request, CancellationToken cancellationToken)
    {
        if(await _presetRepository.Exists(request.UserId, request.SavePresetModel.Name))
        {
            return Result<PresetModel>.Conflict("Preset already exists");
        }

        //if (! await _monitoringClient.GpuExists(request.UserId, request.SavePresetModel.GpuName))
        //{
        //    return Result<PresetModel>.Invalid("GPU with this name wasn`t found");
        //}

        var preset = _mapper.Map<Preset>(request.SavePresetModel);
        preset.UserId = request.UserId;
        await _presetRepository.Save(preset);

        return Result<PresetModel>.SuccessfullyCreated(_mapper.Map<PresetModel>(preset));
    }
}
