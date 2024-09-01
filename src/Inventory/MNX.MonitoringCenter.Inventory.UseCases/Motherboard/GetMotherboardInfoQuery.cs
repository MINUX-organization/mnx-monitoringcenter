using MediatR;
using MNX.Application.UseCases;

namespace MNX.MonitoringCenter.Inventory.UseCases.Motherboard;

using Motherboard = Contracts.Motherboard.Motherboard;

/// <summary>
/// Запрос на получение материнской платы.
/// </summary>
public sealed record GetMotherboardInfoQuery : IRequest<Result<Motherboard>>
{
    /// <summary>
    /// Спецификация инвентаризации.
    /// </summary>
    public InventorySpecification Specification { get; }

    public GetMotherboardInfoQuery(Guid userId, Guid rigId)
    {
        Specification = new InventorySpecification(userId, rigId);
    }
}

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
        var motherboard = await _motherboardRepository.GetByRigId(request.Specification, cancellationToken);

        if (motherboard == null)
        {
            return Result<Motherboard>.Invalid($"Inventory was not found");
        }

        return Result<Motherboard>.Success(motherboard);
    }
}
