using MediatR;
using MNX.Application.UseCases;

namespace MNX.MonitoringCenter.Inventory.UseCases.NetworkAdapter;

using NetworkAdapter = Contracts.NetworkAdapter.NetworkAdapter;

/// <summary>
/// Запрос на получение сетевых адаптеров, подключенных к ригу.
/// </summary>
/// <param name="RigId"> Идентификатор рига. </param>
public sealed record GetNetworkAdaptersInfoQuery(Guid RigId) : IRequest<Result<List<NetworkAdapter>>>;

/// <summary>
/// Обработчик <see cref="GetNetworkAdaptersInfoQuery"/>.
/// </summary>
public class GetNetworkAdaptersInfoQueryHandler :
    IRequestHandler<GetNetworkAdaptersInfoQuery, Result<List<NetworkAdapter>>>
{
    private readonly INetworkAdapterRepository _repository;

    public GetNetworkAdaptersInfoQueryHandler(INetworkAdapterRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<Result<List<NetworkAdapter>>> Handle(GetNetworkAdaptersInfoQuery request,
                                                            CancellationToken cancellationToken)
    {
        var adapters = await _repository.GetList(request.RigId, cancellationToken);

        if (adapters == null)
        {
            return Result<List<NetworkAdapter>>
                .Invalid($"Inventory for rig with id equaled {request.RigId} was not found");
        }

        return Result<List<NetworkAdapter>>.Success(adapters);
    }
}
