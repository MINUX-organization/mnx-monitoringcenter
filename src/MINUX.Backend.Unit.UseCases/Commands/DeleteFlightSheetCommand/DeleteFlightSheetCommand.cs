using Kernel.UseCases;
using MediatR;

namespace MINUX.Backend.Unit.UseCases.Commands.DeleteFlightSheetCommand;

public class DeleteFlightSheetCommand : IRequest<Result<MediatR.Unit>>
{
    public Guid Id { get; }

    public DeleteFlightSheetCommand(Guid id)
    {
        Id = id;
    }
}
