using AutoMapper;
using Kernel.UseCases;
using MediatR;
using MINUX.Backend.Unit.Core;
using MINUX.Backend.Unit.UseCases.Abstractions;

namespace MINUX.Backend.Unit.UseCases.Commands.AddPoolCommand;

public class AddPoolCommandHandler : IRequestHandler<AddPoolCommand, Result<Guid>>
{
    private readonly IMainRepository _repository;

    private readonly IMapper _mapper;

    public AddPoolCommandHandler(IMainRepository repository, IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Result<Guid>> Handle(AddPoolCommand request, CancellationToken cancellationToken)
    {
        if (!await _repository.Cryptocurrencies.Exists(request.CryptocurrencyId))
        {
            return Result<Guid>.NotFound("Cryptocurrency wasn't found");
        }

        var id = await _repository.Pools.Add(_mapper.Map<Pool>(request));
        await _repository.SaveChangesAsync();

        return Result<Guid>.SuccessfullyCreated(id);
    }
}