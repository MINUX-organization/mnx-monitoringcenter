using AutoMapper;
using MediatR;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Pools.AddPool;

/// <summary>
/// Обработчик команды добавления пула
/// </summary>
public class AddPoolCommandHandler : IRequestHandler<AddPoolCommand, Result<PoolModel>>
{
    private readonly IPoolRepository _poolRepository;

    private readonly ICryptocurrencyRepository _cryptocurrencyRepository;

    private readonly IMapper _mapper;

    public AddPoolCommandHandler(IPoolRepository poolRepository,
                                 ICryptocurrencyRepository cryptocurrencyRepository,
                                 IMapper mapper)
    {
        _poolRepository = poolRepository ?? throw new ArgumentNullException(nameof(poolRepository));
        _cryptocurrencyRepository = cryptocurrencyRepository ?? throw new ArgumentNullException(nameof(cryptocurrencyRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Result<PoolModel>> Handle(AddPoolCommand request, CancellationToken cancellationToken)
    {
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

        var pool = _mapper.Map<Pool>(request);
        pool.Id = await _poolRepository.Add(pool).ConfigureAwait(false);
        pool.Cryptocurrency = cryptocurrency;
                
        return Result<PoolModel>.SuccessfullyCreated(_mapper.Map<PoolModel>(pool));
    }
}