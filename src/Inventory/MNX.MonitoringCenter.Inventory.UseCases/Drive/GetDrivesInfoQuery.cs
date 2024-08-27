using MediatR;

namespace MNX.MonitoringCenter.Inventory.UseCases.Drive;

internal class GetDrivesInfoQuery : IStreamRequest<Contracts.Drive.Drive>
{
}
