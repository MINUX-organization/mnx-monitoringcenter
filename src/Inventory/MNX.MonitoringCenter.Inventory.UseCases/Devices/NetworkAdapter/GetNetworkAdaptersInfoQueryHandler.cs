using MediatR;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.NetworkAdapter;

namespace MNX.MonitoringCenter.Inventory.UseCases.Devices.NetworkAdapter;

using NetworkAdapter = Contracts.Devices.NetworkAdapter.NetworkAdapter;

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
        var adapters = await _repository.GetNetworkAdapters(request.Specification, cancellationToken);

        if (adapters == null)
        {
            return Result<List<NetworkAdapter>>.Invalid($"Inventory was not found");
        }

        if (request.IsOnline != null)
        {
            adapters = adapters.Where(x => x.IsOnline == request.IsOnline).ToList();
        }

        return Result<List<NetworkAdapter>>.Success(adapters);
    }
}
