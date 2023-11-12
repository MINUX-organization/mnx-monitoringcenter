using Kernel.UseCases;
using MediatR;

namespace MINUX.Backend.Unit.UseCases.Commands.DeletePresetCommand;

public class DeletePresetCommand : IRequest<Result<MediatR.Unit>>
{
    public Guid Id { get; }

    public DeletePresetCommand(Guid id)
    {
        Id = id;
    }
}