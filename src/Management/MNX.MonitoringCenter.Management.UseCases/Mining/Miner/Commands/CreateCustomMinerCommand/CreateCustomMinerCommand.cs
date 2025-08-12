using MediatR;
using MNX.Application.UseCases.Requests;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Management.Contracts.Miner;
using MNX.MonitoringCenter.Management.UseCases.Mining.Miner.Commands.Models;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Miner.Commands.CreateCustomMinerCommand;

/// <summary>
/// Команда создания пользовательского майнера.
/// </summary>
/// <param name="Model"> Модель ввода данных кастомного майнера. </param>
/// <param name="UserId"> Идентификатор пользователя. </param>
public sealed record CreateCustomMinerCommand(MinerInputModel Model, Guid UserId)
    : IUserableValidatableCommand<MinerModel>;

/// <summary>
/// Обработчик команды <see cref="CreateCustomMinerCommand"/>.
/// </summary>
public class CreateCustomMinerCommandHandler : IRequestHandler<CreateCustomMinerCommand, Result<MinerModel>>
{
    private readonly IMinerRepository _minerRepository;
    private readonly IMinerMapper _minerMapper;

    ///
    public CreateCustomMinerCommandHandler(IMinerRepository minerRepository,
                                           IMinerMapper minerMapper)
    {
        _minerRepository = minerRepository ?? throw new ArgumentNullException(nameof(minerRepository));
        _minerMapper = minerMapper ?? throw new ArgumentNullException(nameof(minerMapper));
    }

    ///
    public async Task<Result<MinerModel>> Handle(CreateCustomMinerCommand request,
                                                 CancellationToken cancellationToken)
    {
        var model = request.Model;
        if (await _minerRepository.Exists(request.UserId,
                                          model.Name,
                                          model.Version,
                                          cancellationToken))
        {
            return Result<MinerModel>.Conflict(
                $"Miner with name {model.Name} and version {model.Version} already exists");
        }

        var miner = _minerMapper.MapToCoreEntity(model, request.UserId);

        await _minerRepository.Add(miner, cancellationToken);

        return Result<MinerModel>.SuccessfullyCreated(_minerMapper.MapToModel(miner));
    }
}