using AutoMapper;
using MediatR;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Management.Contracts;

namespace MNX.MonitoringCenter.Management.UseCases.Pool.Commands.UpdatePool;

using Pool = Core.Pool;

/// <summary>
/// Обработчик команды обновления пула
/// </summary>
public class UpdatePoolCommandHandler : IRequestHandler<UpdatePoolCommand, Result<PoolModel>>
{
    private readonly IPoolRepository _poolRepository;

    private readonly IMapper _mapper;

    public UpdatePoolCommandHandler(IPoolRepository poolRepository,
                                    IMapper mapper)
    {
        _poolRepository = poolRepository ?? throw new ArgumentNullException(nameof(poolRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Result<PoolModel>> Handle(UpdatePoolCommand request, CancellationToken cancellationToken)
    {
        var pool = await _poolRepository.GetAvailableById(request.Id, request.UserId);

        if (pool == null)
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

        if (await _poolRepository.Exists(request.UserId, request.Model.Domain, request.Model.Port))
        {
            return Result<PoolModel>.Invalid("Pool already exists");
        }
        
        await _poolRepository.Update(newPool).ConfigureAwait(false);
        newPool.Cryptocurrency = pool.Cryptocurrency;

        return Result<PoolModel>.Success(_mapper.Map<PoolModel>(newPool));
    }
}
