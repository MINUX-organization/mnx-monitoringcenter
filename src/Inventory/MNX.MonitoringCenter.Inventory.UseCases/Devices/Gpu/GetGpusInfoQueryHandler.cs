using MediatR;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Gpu.GetGpusInfo;

namespace MNX.MonitoringCenter.Inventory.UseCases.Devices.Gpu;

/// <summary>
/// Обработчик <see cref="GetGpusInfoQuery"/>.
/// </summary>
public class GetGpusInfoQueryHandler : IStreamRequestHandler<GetGpusInfoQuery, GpuDetails>
{
    private readonly IGpuRepository _gpuRepository;

    public GetGpusInfoQueryHandler(IGpuRepository gpuRepository)
    {
        _gpuRepository = gpuRepository ?? throw new ArgumentNullException(nameof(gpuRepository));
    }

    public IAsyncEnumerable<GpuDetails> Handle(GetGpusInfoQuery request, CancellationToken cancellationToken)
    {
        return _gpuRepository.GetGpus(request.Specification);
    }
}
