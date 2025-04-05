using MediatR;
using AutoMapper;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Management.UseCases.Mining.Algorithm;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Cryptocurrency.Commands.AddCryptocurrency;

using Cryptocurrency = Core.Mining.Cryptocurrency;

/// <summary>
/// Обработчик команды <see cref="AddCryptocurrencyCommand"/>.
/// </summary>
public class AddCryptocurrencyCommandHandler :
    IRequestHandler<AddCryptocurrencyCommand, Result<CryptocurrencyModel>>
{
    private readonly ICryptocurrencyRepository _cryptocurrencyRepository;

    private readonly IAlgorithmRepository _algorithmRepository;

    private readonly IMapper _mapper;

    public AddCryptocurrencyCommandHandler(ICryptocurrencyRepository cryptocurrencyRepository,
                                           IAlgorithmRepository algorithmRepository,
                                           IMapper mapper)
    {
        _cryptocurrencyRepository = cryptocurrencyRepository ??
            throw new ArgumentNullException(nameof(cryptocurrencyRepository));
        _algorithmRepository = algorithmRepository ??
            throw new ArgumentNullException(nameof(algorithmRepository));
        _mapper = mapper ??
            throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Result<CryptocurrencyModel>> Handle(AddCryptocurrencyCommand request,
                                                          CancellationToken cancellationToken)
    {
        var model = request.Model;

        if (await _cryptocurrencyRepository.Exists(request.UserId,
                                                   request.Model.FullName,
                                                   request.Model.ShortName,
                                                   cancellationToken))
        {
            return Result<CryptocurrencyModel>.Conflict("Cryptocurrency already exists");
        }

        var algorithm = await _algorithmRepository.GetById(model.AlgorithmId,
                                                           request.UserId,
                                                           cancellationToken);

        if (algorithm is null)
        {
            return Result<CryptocurrencyModel>
                .Invalid($"Algorithm with id equaled {model.AlgorithmId} wasn't found");
        }

        var cryptocurrency = _mapper.Map<Cryptocurrency>(request);
        await _cryptocurrencyRepository.Add(cryptocurrency);
        cryptocurrency.Algorithm = algorithm;

        return Result<CryptocurrencyModel>
            .SuccessfullyCreated(_mapper.Map<CryptocurrencyModel>(cryptocurrency));
    }
}