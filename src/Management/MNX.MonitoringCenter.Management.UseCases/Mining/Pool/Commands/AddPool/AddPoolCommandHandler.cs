using MediatR;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Management.UseCases.Mining.Cryptocurrency;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Pool.Commands.AddPool;
/// <summary>
/// Обработчик команды <see cref="AddPoolCommand"/>.
/// </summary>
public class AddPoolCommandHandler : IRequestHandler<AddPoolCommand, Result<PoolModel>>
{
    private readonly IPoolRepository _poolRepository;

    private readonly ICryptocurrencyRepository _cryptocurrencyRepository;

    private readonly IPoolMapper _poolMapper;

    ///
    public AddPoolCommandHandler(IPoolRepository poolRepository,
                                 ICryptocurrencyRepository cryptocurrencyRepository,
                                 IPoolMapper poolMapper)
    {
        _poolRepository = poolRepository ??
            throw new ArgumentNullException(nameof(poolRepository));
        _cryptocurrencyRepository = cryptocurrencyRepository ??
            throw new ArgumentNullException(nameof(cryptocurrencyRepository));
        _poolMapper = poolMapper ??
            throw new ArgumentNullException(nameof(poolMapper));
    }

    ///
    public async Task<Result<PoolModel>> Handle(AddPoolCommand request,
                                                CancellationToken cancellationToken)
    {
        if (await _poolRepository.Exists(request.UserId,
                                         request.Model.Domain,
                                         request.Model.Port,
                                         cancellationToken))
        {
            return Result<PoolModel>.Conflict("Pool already exists");
        }

        var cryptocurrency = await _cryptocurrencyRepository
            .GetAvailableById(request.Model.CryptocurrencyId, request.UserId, cancellationToken);

        if (cryptocurrency is null)
        {
            return Result<PoolModel>
                .Invalid($"Cryptocurrency with id equaled {request.Model.CryptocurrencyId} wasn't found");
        }

        var pool = _poolMapper.MapToCoreEntity(request);
        await _poolRepository.Add(pool);
        pool.Cryptocurrency = cryptocurrency;

        return Result<PoolModel>.SuccessfullyCreated(_poolMapper.MapToModel(pool));
    }
}