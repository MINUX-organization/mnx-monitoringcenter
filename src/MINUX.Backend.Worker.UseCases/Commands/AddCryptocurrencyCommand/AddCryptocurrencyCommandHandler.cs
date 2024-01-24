using AutoMapper;
using Kernel.UseCases;
using MediatR;
using MINUX.Backend.Worker.Core;
using MINUX.Backend.Worker.UseCases.Abstractions;

namespace MINUX.Backend.Worker.UseCases.Commands.AddCryptocurrencyCommand;

/// <summary>
/// Обработчик команды добавления криптовалюты
/// </summary>
public class AddCryptocurrencyCommandHandler : IRequestHandler<AddCryptocurrencyCommand, Result<Guid>>
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

    public async Task<Result<Guid>> Handle(AddCryptocurrencyCommand request, CancellationToken cancellationToken)
    {
        if (await _cryptocurrencyRepository.Exists(request.ShortName, request.FullName))
        {
            return Result<Guid>.Conflict("Cryptocurrency already exists");
        }

        if (!await _algorithmRepository.Exists(request.Algorithm))
        {
            return Result<Guid>.Invalid("Algorithm wasn't found");
        }

        var id = await _cryptocurrencyRepository.Add(_mapper.Map<Cryptocurrency>(request));

        return Result<Guid>.SuccessfullyCreated(id);
    }
}