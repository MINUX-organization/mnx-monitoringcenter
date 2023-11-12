using Kernel.UseCases;
using MediatR;

namespace MINUX.Backend.Unit.UseCases.Commands.DeleteWalletCommand;

public class DeleteWalletCommand : IRequest<Result<MediatR.Unit>>
{
    public Guid Id { get; }

    public DeleteWalletCommand(Guid id)
    {
        Id = id;
    }
}
