using MediatR;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Gpu;

namespace MNX.MonitoringCenter.Inventory.UseCases.RigInventory.Devices.Gpu;

using Gpu = Contracts.Gpu.Gpu;

/// <summary>
/// Обработчик <see cref="GetGpusInfoQuery"/>.
/// </summary>
public class GetGpusInfoQueryHandler : IStreamRequestHandler<GetGpusInfoQuery, Gpu>
{
    private readonly IGpuRepository _gpuRepository;

    public GetGpusInfoQueryHandler(IGpuRepository gpuRepository)
    {
        _gpuRepository = gpuRepository ?? throw new ArgumentNullException(nameof(gpuRepository));
    }

    public IAsyncEnumerable<Gpu> Handle(GetGpusInfoQuery request, CancellationToken cancellationToken)
    {
        return _gpuRepository.GetGpus(request.Specification);
    }
}
