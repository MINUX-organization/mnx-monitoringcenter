using MediatR;
using AutoMapper;
using MNX.Application.UseCases.Results;
using MNX.Application.UseCases.Requests;
using MNX.MonitoringCenter.Management.UseCases.Mining.Miner.Commands.Models;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Miner.Commands.EditMinerCommand;

/// <summary>
/// Команда редактирования майнера.
/// </summary>
/// <param name="Model"> Модель ввода майнера. </param>
/// <param name="MinerId"> Идентификатор майнера. </param>
/// <param name="UserId"> ИДентификатор пользователя. </param>
public sealed record EditMinerCommand(MinerInputModel Model, Guid MinerId, Guid UserId)
    : IUserableValidatableCommand<Unit>;

/// <summary>
/// Обработчик команды <see cref="EditMinerCommand"/>.
/// </summary>
public class EditMinerCommandHandler : IRequestHandler<EditMinerCommand, Result<Unit>>
{
    private readonly IMinerRepository _minerRepository;
    private readonly IMapper _mapper;

    public EditMinerCommandHandler(IMinerRepository minerRepository, IMapper mapper)
    {
        _minerRepository = minerRepository ?? throw new ArgumentNullException(nameof(minerRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Result<Unit>> Handle(EditMinerCommand request, CancellationToken cancellationToken)
    {
        if (!await _minerRepository.Exists(request.MinerId, cancellationToken))
        {
            return Result<Unit>.Invalid($"Miner with id equaled {request.MinerId} was not found");
        }

        var miner = await _minerRepository.GetMinerById(request.MinerId, cancellationToken);

        if (await _minerRepository.Exists(request.MinerId, request.UserId, request.Model.Name, cancellationToken))
        {
            return Result<Unit>.Conflict($"Miner with name {request.Model.Name} already exists");
        }

        _mapper.Map(miner, request.Model);
        await _minerRepository.Edit(miner!, cancellationToken);
        return Result<Unit>.Empty();
    }
}