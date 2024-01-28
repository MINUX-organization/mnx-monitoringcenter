using Kernel.UseCases;
using MediatR;
using MINUX.Backend.Worker.Core.HardwareParameters;
using MINUX.Backend.Worker.UseCases.Abstractions;

namespace MINUX.Backend.Worker.UseCases.Queries.HardwareParameters.GetSystemInfo;

/// <summary>
/// Обработчик запроса на получение информации о системе
/// </summary>
public class GetSystemInfoQueryHandler : IRequestHandler<GetSystemInfoQuery, Result<SystemInfo>>
{
    private readonly IHardwareParametersRepository _repository;

    public GetSystemInfoQueryHandler(IHardwareParametersRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<Result<SystemInfo>> Handle(GetSystemInfoQuery request, CancellationToken cancellationToken)
    {
        return Result<SystemInfo>.Success(await _repository.GetSystemInfo());
    }
}
