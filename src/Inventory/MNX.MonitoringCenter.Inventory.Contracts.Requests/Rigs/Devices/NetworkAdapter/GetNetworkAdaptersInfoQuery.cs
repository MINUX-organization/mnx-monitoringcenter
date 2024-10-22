using MediatR;
using MNX.Application.UseCases;

namespace MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.NetworkAdapter;

using NetworkAdapter = Contracts.Devices.NetworkAdapter.NetworkAdapter;

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
        Specification = new InventorySpecification(userId, rigId, true);
        IsOnline = isOnline;
    }
}
