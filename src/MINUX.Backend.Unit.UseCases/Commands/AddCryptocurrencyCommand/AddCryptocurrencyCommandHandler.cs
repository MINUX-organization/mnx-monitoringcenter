using AutoMapper;
using Kernel.UseCases;
using MediatR;
using MINUX.Backend.Unit.Core;
using MINUX.Backend.Unit.UseCases.Abstractions;

namespace MINUX.Backend.Unit.UseCases.Commands.AddCryptocurrencyCommand;

/// <summary>
/// Обработчик команды добавления криптовалюты
/// </summary>
public class AddCryptocurrencyCommandHandler : IRequestHandler<AddCryptocurrencyCommand, Result<Guid>>
{
    private readonly IMainRepository _repository;

    private readonly IMapper _mapper;

    public AddCryptocurrencyCommandHandler(IMainRepository repository, IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Result<Guid>> Handle(AddCryptocurrencyCommand request, CancellationToken cancellationToken)
    {
        if (await _repository.Cryptocurrencies.Exists(request.ShortName, request.FullName))
        {
            return Result<Guid>.Conflict("Cryptocurrency already exists");
        }

        if (!await _repository.Algorithms.Exists(request.Algorithm))
        {
            return Result<Guid>.NotFound("Algorithm wasn't found");
        }

        var id = await _repository.Cryptocurrencies.Add(_mapper.Map<Cryptocurrency>(request));
        await _repository.SaveChangesAsync();

        return Result<Guid>.SuccessfullyCreated(id);
    }
}