using Kernel.UseCases;
using MediatR;
using MINUX.Backend.Unit.UseCases.Abstractions;

namespace MINUX.Backend.Unit.UseCases.Commands.DeleteCommand;

public class DeleteCommandHandler : IRequestHandler<DeleteCommand, Result<MediatR.Unit>>
{
    private readonly IMainRepository _repository;

    public DeleteCommandHandler(IMainRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<MediatR.Unit>> Handle(DeleteCommand request, CancellationToken cancellationToken)
    {
        var repository = GetRepository(request.TypeCommand);
        throw new NotImplementedException();
    }

    private IRepository? GetRepository(DeleteCommandEnum typeCommand)
    {
        var factory = new DeleteCommandFactory(_repository);
        return factory.GetRepository(typeCommand);
    }
}