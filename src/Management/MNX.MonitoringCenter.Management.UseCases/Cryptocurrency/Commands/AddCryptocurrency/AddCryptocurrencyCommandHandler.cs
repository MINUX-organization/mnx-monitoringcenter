using AutoMapper;
using MediatR;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Management.UseCases.Algorithm;

namespace MNX.MonitoringCenter.Management.UseCases.Cryptocurrency.Commands.AddCryptocurrency;

using Cryptocurrency = Core.Cryptocurrency;

/// <summary>
/// Обработчик команды добавления криптовалюты
/// </summary>
public class AddCryptocurrencyCommandHandler : IRequestHandler<AddCryptocurrencyCommand, Result<CryptocurrencyModel>>
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

    public async Task<Result<CryptocurrencyModel>> Handle(AddCryptocurrencyCommand request, CancellationToken cancellationToken)
    {
        if (await _cryptocurrencyRepository.Exists(request.UserId, request.Model.FullName, request.Model.ShortName))
        {
            return Result<CryptocurrencyModel>.Conflict("Cryptocurrency already exists");
        }

        if (!await _algorithmRepository.Exists(request.Model.Algorithm))
        {
            return Result<CryptocurrencyModel>.Invalid("Algorithm wasn't found");
        }

        var cryptocurrency = _mapper.Map<Cryptocurrency>(request);
        await _cryptocurrencyRepository.Add(cryptocurrency);

        return Result<CryptocurrencyModel>.SuccessfullyCreated(_mapper.Map<CryptocurrencyModel>(cryptocurrency));
    }
}