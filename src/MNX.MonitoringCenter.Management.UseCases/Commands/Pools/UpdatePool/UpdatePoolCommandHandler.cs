using AutoMapper;
using Kernel.UseCases;
using MediatR;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Pools.UpdatePool;

/// <summary>
/// Обработчик команды обновления пула
/// </summary>
public class UpdatePoolCommandHandler : IRequestHandler<UpdatePoolCommand, Result<Unit>>
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

    public async Task<Result<Unit>> Handle(UpdatePoolCommand request, CancellationToken cancellationToken)
    {
        var pool = await _poolRepository.GetById(request.Id);

        if (pool == null)
        {
            return Result<Unit>.Invalid("Pool with this id wasn`t found");
        }

        if (PoolsIsEquals(pool, request.Model))
        {
            return Result<Unit>.Empty();
        }

        if (await _poolRepository.Exists(request.Model.Domain, request.Model.Port))
        {
            return Result<Unit>.Invalid("Pool already exists");
        }

        if (! await _cryptocurrencyRepository.Exists(request.Model.CryptocurrencyFullName))
        {
            return Result<Unit>.Invalid("Cryptocurrency wasn't found");
        }

        var newPool = _mapper.Map<Pool>(request.Model);
        newPool.Id = request.Id;
        await _poolRepository.Update(newPool);
        return Result<Unit>.Empty();
    }

    private static bool PoolsIsEquals(Pool pool, PoolModel newPool)
    {
        return pool.Domain == newPool.Domain && pool.Port == newPool.Port;
    }
}
