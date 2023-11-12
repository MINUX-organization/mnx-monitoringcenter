using Kernel.UseCases;
using MediatR;

namespace MINUX.Backend.Unit.UseCases.Commands.DeleteCryptocurrencyCommand;

public class DeleteCryptocurrencyCommand : IRequest<Result<MediatR.Unit>>
{
    public Guid Id { get; }

    public DeleteCryptocurrencyCommand(Guid id)
    {
        Id = id;
    }
}
