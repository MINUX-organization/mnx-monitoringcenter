using MediatR;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Gpu;

namespace MNX.MonitoringCenter.Inventory.UseCases.RigInventory.Devices.Gpu;

using Gpu = Contracts.Gpu.Gpu;

/// <summary>
/// Обработчик <see cref="GetSliceGpuInventoryForAPeriodQuery"/>.
/// </summary>
public class GetSliceGpuInventoryForAPeriodQueryHandler
    : IStreamRequestHandler<GetSliceGpuInventoryForAPeriodQuery, List<Gpu>>
{
    private readonly IGpuRepository _gpuRepository;

    public GetSliceGpuInventoryForAPeriodQueryHandler(IGpuRepository gpuRepository)
    {
        _gpuRepository = gpuRepository ?? throw new ArgumentNullException(nameof(gpuRepository));
    }

    public IAsyncEnumerable<List<Gpu>> Handle(GetSliceGpuInventoryForAPeriodQuery request,
                                              CancellationToken cancellationToken)
    {
        return _gpuRepository.GetGpusSliceForAPeriod(request.Specification, request.StartPeriod, request.EndPeriod);
    }
}
