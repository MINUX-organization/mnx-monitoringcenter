using MediatR;
using MNX.Application.UseCases;

namespace MNX.MonitoringCenter.Inventory.UseCases.NetworkAdapter;

using NetworkAdapter = Contracts.NetworkAdapter.NetworkAdapter;

/// <summary>
/// Запрос на получение сетевых адаптеров, подключенных к ригу.
/// </summary>
public sealed record GetNetworkAdaptersInfoQuery : IRequest<Result<List<NetworkAdapter>>>
{
    /// <summary>
    /// Спецификация инвентаризации.
    /// </summary>
    public InventorySpecification Specification { get; }

    /// <summary>
    /// Признак того, что адаптер подключен к сети Интернет.
    /// </summary>
    public bool? IsOnline { get; }

    public GetNetworkAdaptersInfoQuery(Guid userId, Guid rigId, bool? isOnline = null)
    {
        Specification = new InventorySpecification(userId, rigId);
        IsOnline = isOnline;
    }
}

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
        var adapters = await _repository.GetList(request.Specification, cancellationToken);

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
