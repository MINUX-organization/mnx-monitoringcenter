using MediatR;
using MNX.Application.UseCases;

namespace MNX.MonitoringCenter.Inventory.UseCases.Motherboard;

using Motherboard = Contracts.Motherboard.Motherboard;

/// <summary>
/// Запрос на получение материнской платы.
/// </summary>
/// <param name="UserId"> Идентификатор пользователя. </param>
/// <param name="RigId"> Идентификатор рига. </param>
public sealed record GetMotherboardInfoQuery(Guid UserId, Guid RigId) : IRequest<Result<Motherboard>>;

/// <summary>
/// Обработчик <see cref="GetMotherboardInfoQuery"/>.
/// </summary>
public class GetMotherboardInfoQueryHandler : IRequestHandler<GetMotherboardInfoQuery, Result<Motherboard>>
{
    private readonly IMotherboardRepository _motherboardRepository;

    public GetMotherboardInfoQueryHandler(IMotherboardRepository motherboardRepository)
    {
        _motherboardRepository = motherboardRepository
            ?? throw new ArgumentNullException(nameof(motherboardRepository));
    }

    public async Task<Result<Motherboard>> Handle(GetMotherboardInfoQuery request,
                                                  CancellationToken cancellationToken)
    {
        var motherboard = await _motherboardRepository.GetByRigId(request.RigId, request.UserId, cancellationToken);

        if (motherboard == null)
        {
            return Result<Motherboard>.Invalid($"Inventory was not found");
        }

        return Result<Motherboard>.Success(motherboard);
    }
}
