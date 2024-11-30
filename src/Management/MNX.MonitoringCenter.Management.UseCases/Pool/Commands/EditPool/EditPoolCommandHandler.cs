using AutoMapper;
using MediatR;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Management.Contracts;

namespace MNX.MonitoringCenter.Management.UseCases.Pool.Commands.EditPool;

using Pool = Core.Pool;

/// <summary>
/// Обработчик команды обновления пула
/// </summary>
public class EditPoolCommandHandler : IRequestHandler<EditPoolCommand, Result<PoolModel>>
{
    private readonly IPoolRepository _poolRepository;

    private readonly IMapper _mapper;

    public EditPoolCommandHandler(IPoolRepository poolRepository,
                                    IMapper mapper)
    {
        _poolRepository = poolRepository ?? throw new ArgumentNullException(nameof(poolRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Result<PoolModel>> Handle(EditPoolCommand request, CancellationToken cancellationToken)
    {
        var pool = await _poolRepository.GetAvailableById(request.Id, request.UserId, cancellationToken);

        if (pool is null)
        {
            return Result<PoolModel>.Invalid("Pool with this id wasn`t found");
        }

        if (pool.CryptocurrencyId != request.Model.CryptocurrencyId)
        {
            return Result<PoolModel>.Invalid("You cannot change the cryptocurrency");
        }

        var newPool = _mapper.Map<Pool>(request);

        if (pool.Equals(newPool))
        {
            return Result<PoolModel>.Success(_mapper.Map<PoolModel>(pool));
        }

        if (( newPool.Domain != pool.Domain || newPool.Port != pool.Port) &&
            await _poolRepository.Exists(request.UserId, newPool.Domain, newPool.Port, cancellationToken))
        {
            return Result<PoolModel>.Conflict("Pool already exists");
        }
        
        await _poolRepository.Update(newPool);
        newPool.Cryptocurrency = pool.Cryptocurrency;

        return Result<PoolModel>.Success(_mapper.Map<PoolModel>(newPool));
    }
}
