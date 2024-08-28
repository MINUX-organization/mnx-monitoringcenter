using MediatR;

namespace MNX.MonitoringCenter.Inventory.UseCases.Gpu;

using Gpu = Contracts.Gpu.Gpu;

/// <summary>
/// Запрос на получение списка видеокарт.
/// </summary>
/// <param name="Specification"></param>
public sealed record GetGpusInfoQuery(DeviceSpecification Specification) : IStreamRequest<Gpu>;

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
        return _gpuRepository.GetList(request.Specification);
    }
}
