using MediatR;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Inventory.Contracts.Queries.Motherboard;

namespace MNX.MonitoringCenter.Inventory.UseCases.Motherboard;

using Motherboard = Contracts.Motherboard.Motherboard;

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
