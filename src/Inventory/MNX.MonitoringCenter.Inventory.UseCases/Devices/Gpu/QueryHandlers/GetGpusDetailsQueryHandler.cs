using MediatR;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Gpu.GetGpusDetails;

namespace MNX.MonitoringCenter.Inventory.UseCases.Devices.Gpu.QueryHandlers;

/// <summary>
/// Обработчик <see cref="GetGpusDetailsQuery"/>.
/// </summary>
public class GetGpusDetailsQueryHandler : IStreamRequestHandler<GetGpusDetailsQuery, GpuDetails>
{
    private readonly IGpuRepository _gpuRepository;

    public GetGpusDetailsQueryHandler(IGpuRepository gpuRepository)
    {
        _gpuRepository = gpuRepository ?? throw new ArgumentNullException(nameof(gpuRepository));
    }

    public IAsyncEnumerable<GpuDetails> Handle(GetGpusDetailsQuery request, CancellationToken cancellationToken)
    {
        return _gpuRepository.GetGpus(request.Specification);
    }
}
