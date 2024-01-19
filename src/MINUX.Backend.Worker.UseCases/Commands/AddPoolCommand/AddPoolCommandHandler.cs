using AutoMapper;
using Kernel.UseCases;
using MediatR;
using MINUX.Backend.Worker.Core;
using MINUX.Backend.Worker.UseCases.Abstractions;

namespace MINUX.Backend.Worker.UseCases.Commands.AddPoolCommand;

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
        if (!await _cryptocurrencyRepository.Exists(request.CryptocurrencyId))
        {
            return Result<Guid>.NotFound("Cryptocurrency wasn't found");
        }

        var id = await _poolRepository.Add(_mapper.Map<Pool>(request));

        return Result<Guid>.SuccessfullyCreated(id);
    }
}