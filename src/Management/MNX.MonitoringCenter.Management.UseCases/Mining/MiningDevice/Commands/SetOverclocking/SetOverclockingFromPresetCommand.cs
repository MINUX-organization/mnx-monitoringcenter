using AutoMapper;
using MediatR;
using MNX.Application.UseCases.Requests;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Management.Contracts.Presets;
using MNX.MonitoringCenter.Management.UseCases.Overclocking.Presets;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.MiningDevice.Commands.SetOverclocking;

/// <summary>
/// Команда на установку разгона на майнинг устройство через пресет.
/// </summary>
/// <param name="UserId"> Идентификатор пользователя. </param>
/// <param name="PresetId"> Идентификатор пресета. </param>
/// <param name="DeviceId"> Идентификатор майнинг устройства. </param>
public sealed record SetOverclockingFromPresetCommand(Guid UserId, Guid PresetId, Guid DeviceId)
    : IUserableValidatableCommand<Guid>;


/// <summary>
/// Обработчик <see cref="SetOverclockingFromPresetCommand"/>.
/// </summary>
public class SetOverclockingFromPresetCommandHandler :
    IRequestHandler<SetOverclockingFromPresetCommand, Result<Guid>>
{
    private readonly IPresetRepository _presetRepository;

    private readonly IMediator _mediator;

    private readonly IMapper _mapper;

    private readonly IMiningDeviceRepository _miningDeviceRepository;

    public SetOverclockingFromPresetCommandHandler(IPresetRepository repository,
                                                   IMediator mediator,
                                                   IMapper mapper,
                                                   IMiningDeviceRepository miningDeviceRepository)
    {
        _presetRepository = repository ??
            throw new ArgumentNullException(nameof(repository));
        _mediator = mediator ??
            throw new ArgumentNullException(nameof(mediator));
        _mapper = mapper ??
            throw new ArgumentNullException(nameof(mapper));
        _miningDeviceRepository = miningDeviceRepository ??
            throw new ArgumentNullException(nameof(miningDeviceRepository));
    }

    public async Task<Result<Guid>> Handle(SetOverclockingFromPresetCommand request, CancellationToken cancellationToken)
    {
        var preset = await _presetRepository.GetAvailableById(request.PresetId, request.UserId, cancellationToken);

        var presetResult = preset == null
            ? Result<PresetModel>.Invalid("Preset with this id must exist")
            : Result<PresetModel>.Success(_mapper.Map<PresetModel>(preset));

        if (!presetResult.IsSuccess)
        {
            return Result<Guid>.Invalid(presetResult.Errors!);
        }

        await _miningDeviceRepository.SetPreset(request.DeviceId, request.PresetId);

        return Result<Guid>.Success(presetResult.GetValue().Id);
    }
}