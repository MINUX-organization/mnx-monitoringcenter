using AutoMapper;
using Kernel.UseCases;
using MediatR;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Crypto.AddCryptocurrency;

/// <summary>
/// Обработчик команды добавления криптовалюты
/// </summary>
public class AddCryptocurrencyCommandHandler : IRequestHandler<AddCryptocurrencyCommand, Result<Cryptocurrency>>
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

    public async Task<Result<Cryptocurrency>> Handle(AddCryptocurrencyCommand request, CancellationToken cancellationToken)
    {
        if (await _cryptocurrencyRepository.Exists(request.UserId, request.Model.FullName, request.Model.ShortName))
        {
            return Result<Cryptocurrency>.Conflict("Cryptocurrency already exists");
        }

        if (!await _algorithmRepository.Exists(request.Model.Algorithm))
        {
            return Result<Cryptocurrency>.Invalid("Algorithm wasn't found");
        }

        var cryptocurrency = _mapper.Map<Cryptocurrency>(request.Model);
        cryptocurrency.UserId = request.UserId;
        await _cryptocurrencyRepository.Add(cryptocurrency);

        return Result<Cryptocurrency>.SuccessfullyCreated(cryptocurrency);
    }
}