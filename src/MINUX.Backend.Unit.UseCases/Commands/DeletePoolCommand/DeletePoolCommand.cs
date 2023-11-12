using Kernel.UseCases;
using MediatR;

namespace MINUX.Backend.Unit.UseCases.Commands.DeletePoolCommand;

public class DeletePoolCommand : IRequest<Result<MediatR.Unit>>
{
    public Guid Id { get; }

    public DeletePoolCommand(Guid id)
    {
        Id = id;
    }
}