using AutoMapper;
using MediatR;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Management.UseCases.Mining.Cryptocurrency;
using MNX.MonitoringCenter.Management.UseCases.Mining.Pool;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Pool.Commands.AddPool;

using Pool = Core.Mining.Pool;

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
        if (await _poolRepository.Exists(request.UserId, request.Model.Domain, request.Model.Port, cancellationToken))
        {
            return Result<PoolModel>.Conflict("Pool already exists");
        }

        var cryptocurrency = await _cryptocurrencyRepository
            .GetAvailableById(request.Model.CryptocurrencyId, request.UserId, cancellationToken);

        if (cryptocurrency is null)
        {
            return Result<PoolModel>.Invalid("Cryptocurrency wasn't found");
        }

        var pool = _mapper.Map<Pool>(request);
        await _poolRepository.Add(pool);
        pool.Cryptocurrency = cryptocurrency;

        return Result<PoolModel>.SuccessfullyCreated(_mapper.Map<PoolModel>(pool));
    }
}