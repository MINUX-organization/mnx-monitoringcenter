using MediatR;
using MNX.Application.UseCases.Requests;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Management.UseCases.Overclocking;
using MNX.MonitoringCenter.Management.UseCases.Overclocking.Presets.Queries;

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
public class SetOverclockingFromPresetCommandHandler :
    SaveOverclockingBaseHandler,
    IRequestHandler<SetOverclockingFromPresetCommand, Result<Guid>>
{

    public SetOverclockingFromPresetCommandHandler(IMediator mediator) : base(mediator)
    {
    }

    public async Task<Result<Guid>> Handle(SetOverclockingFromPresetCommand request, CancellationToken cancellationToken)
    {
        var preset = await _mediator.Send(new GetPresetsByIdQuery(request.PresetId, request.UserId));
        if (!preset.IsSuccess)
        {
            return Result<Guid>.Invalid(preset.Errors);
        }
        var response = await _mediator.Send(new SetOverclockingCommand(request.UserId, preset.GetValue().Overclocking, request.DeviceIds));
        if (!response.IsSuccess)
        {
            return Result<Guid>.Invalid(response.Errors);
        }
        return Result<Guid>.Success(preset.GetValue().Id);
    }
}