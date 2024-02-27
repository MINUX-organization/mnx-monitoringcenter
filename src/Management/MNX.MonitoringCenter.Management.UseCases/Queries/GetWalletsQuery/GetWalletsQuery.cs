using MediatR;
using MNX.MonitoringCenter.Management.Contracts;

namespace MNX.MonitoringCenter.Management.UseCases.Queries.GetWalletsQuery;

/// <summary>
/// Модель запроса списка кошельков
/// </summary>
public class GetWalletsQuery : IStreamRequest<WalletModel>
{
}