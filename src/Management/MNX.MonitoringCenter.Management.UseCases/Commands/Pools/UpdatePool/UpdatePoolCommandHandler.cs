using AutoMapper;
using Kernel.UseCases;
using MediatR;
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
        var pool = await _poolRepository.GetById(request.Id);

        if (pool == null)
        {
            return Result<PoolModel>.Invalid("Pool with this id wasn`t found");
        }

        if (PoolsIsEquals(pool, request.Model))
        {
            return Result<PoolModel>.Success(_mapper.Map<PoolModel>(pool));
        }

        if (await _poolRepository.Exists(request.Model.Domain, request.Model.Port))
        {
            return Result<PoolModel>.Invalid("Pool already exists");
        }

        if ((await _cryptocurrencyRepository.GetById(request.Model.CryptocurrencyId)) is null)
        {
            return Result<PoolModel>.Invalid("Cryptocurrency wasn't found");
        }

        var newPool = _mapper.Map<Pool>(request.Model);
        newPool.Id = request.Id;
        await _poolRepository.Update(newPool);
        return Result<PoolModel>.Success(_mapper.Map<PoolModel>(newPool));
    }

    private static bool PoolsIsEquals(Pool pool, PoolInputModel newPool)
    {
        return pool.Domain == newPool.Domain && pool.Port == newPool.Port;
    }
}
