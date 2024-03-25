using AutoMapper;
using Kernel.UseCases;
using MediatR;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Presets.SavePreset;

/// <summary>
/// Обработчик команды сохранения пресета для выбранной серии GPU
/// </summary>
public class SavePresetCommandHandler : IRequestHandler<SavePresetCommand, Result<Guid>>
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

    public async Task<Result<Guid>> Handle(SavePresetCommand request, CancellationToken cancellationToken)
    {
        if (! await _monitoringClient.GpuExists(request.UserId, request.SavePresetModel.GpuName))
        {
            return Result<Guid>.Invalid("GPU with this name wasn`t found");
        }

        var preset = _mapper.Map<Preset>(request);
        var id = await _presetRepository.Save(preset);
        return Result<Guid>.SuccessfullyCreated(id);
    }
}
