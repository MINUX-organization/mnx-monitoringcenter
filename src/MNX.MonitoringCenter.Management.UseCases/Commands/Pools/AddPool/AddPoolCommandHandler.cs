using AutoMapper;
using Kernel.UseCases;
using MediatR;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Pools.AddPool;

/// <summary>
/// Обработчик команды добавления пула
/// </summary>
public class AddPoolCommandHandler : IRequestHandler<AddPoolCommand, Result<Guid>>
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

    public async Task<Result<Guid>> Handle(AddPoolCommand request, CancellationToken cancellationToken)
    {
        if (await _poolRepository.Exists(request.Model.Domain, request.Model.Port))
        {
            return Result<Guid>.Invalid("Pool already exists");
        }

        if (! await _cryptocurrencyRepository.Exists(request.Model.CryptocurrencyFullName))
        {
            return Result<Guid>.Invalid("Cryptocurrency wasn't found");
        }

        var id = await _poolRepository.Add(_mapper.Map<Pool>(request.Model));

        return Result<Guid>.SuccessfullyCreated(id);
    }
}