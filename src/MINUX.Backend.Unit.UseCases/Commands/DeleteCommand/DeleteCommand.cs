using Kernel.UseCases;
using MediatR;

namespace MINUX.Backend.Unit.UseCases.Commands.DeleteCommand;

public class DeleteCommand : IRequest<Result<MediatR.Unit>>
{
    public DeleteCommandEnum TypeCommand { get; }

    public Guid Id { get; }

    public DeleteCommand(DeleteCommandEnum typeCommand, Guid id)
    {
        TypeCommand = typeCommand;
        Id = id;
    }
}