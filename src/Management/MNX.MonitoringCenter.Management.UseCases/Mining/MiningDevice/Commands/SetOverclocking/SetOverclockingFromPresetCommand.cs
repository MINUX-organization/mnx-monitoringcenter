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
/// <param name="DeviceIds"> Идентификатор майнинг устройства. </param>
public sealed record SetOverclockingFromPresetCommand(Guid UserId, Guid PresetId, Guid DeviceIds)
    : IUserableValidatableCommand<Guid>;


/// <summary>
/// Обработчик <see cref="SetOverclockingFromPresetCommand"/>.
/// </summary>
public class SetOverclockingFromPresetCommandHandler : IRequestHandler<SetOverclockingFromPresetCommand, Result<Guid>>
{
    private readonly IPresetRepository _repository;

    private readonly IMediator _mediator;

    private readonly IMapper _mapper;

    public SetOverclockingFromPresetCommandHandler(IPresetRepository repository, IMediator mediator, IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Result<Guid>> Handle(SetOverclockingFromPresetCommand request, CancellationToken cancellationToken)
    {
        var preset = _repository.GetAvailableById(request.PresetId, request.UserId, cancellationToken);
        var presetResult = preset.Result == null
            ? Result<PresetModel>.Invalid("Preset with this id must exist")
            : Result<PresetModel>.Success(_mapper.Map<PresetModel>(preset.Result));
        if (!presetResult.IsSuccess)
        {
            return Result<Guid>.Invalid(presetResult.Errors);
        }

        var response = await _mediator.Send(new SetOverclockingCommand(request.UserId, presetResult.GetValue().Overclocking, request.DeviceIds));
        return response.IsSuccess
            ? Result<Guid>.Success(presetResult.GetValue().Id)
            : Result<Guid>.Invalid(response.Errors);
    }
}