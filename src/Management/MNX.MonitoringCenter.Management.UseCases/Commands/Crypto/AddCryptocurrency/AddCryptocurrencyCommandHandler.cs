using AutoMapper;
using Kernel.UseCases;
using MediatR;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Crypto.AddCryptocurrency;

/// <summary>
/// Обработчик команды добавления криптовалюты
/// </summary>
public class AddCryptocurrencyCommandHandler : IRequestHandler<AddCryptocurrencyCommand, Result<Unit>>
{
    private readonly ICryptocurrencyRepository _cryptocurrencyRepository;

    private readonly IAlgorithmRepository _algorithmRepository;

    private readonly IMapper _mapper;

    public AddCryptocurrencyCommandHandler(ICryptocurrencyRepository cryptocurrencyRepository,
                                           IAlgorithmRepository algorithmRepository,
                                           IMapper mapper)
    {
        _cryptocurrencyRepository = cryptocurrencyRepository ?? throw new ArgumentNullException(nameof(cryptocurrencyRepository));
        _algorithmRepository = algorithmRepository ?? throw new ArgumentNullException(nameof(algorithmRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Result<Unit>> Handle(AddCryptocurrencyCommand request, CancellationToken cancellationToken)
    {
        if (await _cryptocurrencyRepository.Exists(request.FullName, request.ShortName))
        {
            return Result<Unit>.Conflict("Cryptocurrency already exists");
        }

        if (!await _algorithmRepository.Exists(request.Algorithm))
        {
            return Result<Unit>.Invalid("Algorithm wasn't found");
        }

        await _cryptocurrencyRepository.Add(_mapper.Map<Cryptocurrency>(request));

        return Result<Unit>.SuccessfullyCreated(Unit.Value);
    }
}