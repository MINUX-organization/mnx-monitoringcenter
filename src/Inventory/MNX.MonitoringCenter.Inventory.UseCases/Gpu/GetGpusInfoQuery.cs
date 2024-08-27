using MediatR;

namespace MNX.MonitoringCenter.Inventory.UseCases.Gpu;

using Gpu = Contracts.Gpu.Gpu;

internal class GetGpusInfoQuery : IStreamRequest<Gpu>
{
}
