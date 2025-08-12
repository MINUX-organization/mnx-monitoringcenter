using MediatR;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Management.Contracts;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Pool.Commands.EditPool;
/// <summary>
/// Обработчик команды <see cref="EditPoolCommand"/>.
/// </summary>
public class EditPoolCommandHandler : IRequestHandler<EditPoolCommand, Result<PoolModel>>
{
    private readonly IPoolRepository _poolRepository;

    private readonly IPoolMapper _poolMapper;

    ///
    public EditPoolCommandHandler(IPoolRepository poolRepository,
                                  IPoolMapper poolMapper)
    {
        _poolRepository = poolRepository ?? throw new ArgumentNullException(nameof(poolRepository));
        _poolMapper = poolMapper ?? throw new ArgumentNullException(nameof(poolMapper));
    }

    ///
    public async Task<Result<PoolModel>> Handle(EditPoolCommand request, CancellationToken cancellationToken)
    {
        var pool = await _poolRepository.GetAvailableById(request.Id, request.UserId, cancellationToken);

        if (pool is null)
            return Result<PoolModel>.Invalid($"Pool with id equaled {request.Id} wasn`t found");

        if (pool.IsDomain())
            return Result<PoolModel>.Invalid("Domain pools cannot be edited");

        if (pool.CryptocurrencyId != request.Model.CryptocurrencyId)
            return Result<PoolModel>.Invalid("You cannot change the cryptocurrency");

        var newPool = _poolMapper.MapToCoreEntity(request);

        if (pool.Equals(newPool))
        {
            return Result<PoolModel>.Success(_poolMapper.MapToModel(pool));
        }

        if ((newPool.Domain != pool.Domain || newPool.Port != pool.Port) &&
            await _poolRepository.Exists(request.UserId, newPool.Domain, newPool.Port, cancellationToken))
        {
            return Result<PoolModel>.Conflict("Pool already exists");
        }

        await _poolRepository.Update(newPool);
        newPool.Cryptocurrency = pool.Cryptocurrency;

        return Result<PoolModel>.Success(_poolMapper.MapToModel(newPool));
    }
}
