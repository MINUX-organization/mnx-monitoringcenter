using AutoMapper;
using MediatR;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Pools.UpdatePool;

/// <summary>
/// Обработчик команды обновления пула
/// </summary>
public class UpdatePoolCommandHandler : IRequestHandler<UpdatePoolCommand, Result<PoolModel>>
{
    private readonly IPoolRepository _poolRepository;

    private readonly ICryptocurrencyRepository _cryptocurrencyRepository;

    private readonly IMapper _mapper;

    public UpdatePoolCommandHandler(IPoolRepository poolRepository,
                                    ICryptocurrencyRepository cryptocurrencyRepository,
                                    IMapper mapper)
    {
        _poolRepository = poolRepository ?? throw new ArgumentNullException(nameof(poolRepository));
        _cryptocurrencyRepository = cryptocurrencyRepository ?? throw new ArgumentNullException(nameof(cryptocurrencyRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Result<PoolModel>> Handle(UpdatePoolCommand request, CancellationToken cancellationToken)
    {
        var pool = await _poolRepository.GetAvailableById(request.Id, request.UserId);

        if (pool == null)
        {
            return Result<PoolModel>.Invalid("Pool with this id wasn`t found");
        }

        if (PoolsIsEquals(pool, request.Model))
        {
            return pool.CryptocurrencyId == request.Model.CryptocurrencyId
                ? Result<PoolModel>.Success(_mapper.Map<PoolModel>(pool))
                : Result<PoolModel>.Invalid("You can't change only the cryptocurrency");
        }

        if (await _poolRepository.Exists(request.UserId, request.Model.Domain, request.Model.Port))
        {
            return Result<PoolModel>.Invalid("Pool already exists");
        }

        var cryptocurrency = await _cryptocurrencyRepository
            .GetAvailableById(request.Model.CryptocurrencyId, request.UserId);

        if (cryptocurrency is null)
        {
            return Result<PoolModel>.Invalid("Cryptocurrency wasn't found");
        }

        var newPool = _mapper.Map<Pool>(request);
        await _poolRepository.Update(newPool).ConfigureAwait(false);
        newPool.Cryptocurrency = cryptocurrency;

        return Result<PoolModel>.Success(_mapper.Map<PoolModel>(newPool));
    }

    private static bool PoolsIsEquals(Pool pool, PoolInputModel newPool)
    {
        return pool.Domain == newPool.Domain &&
               pool.Port == newPool.Port;
    }
}
