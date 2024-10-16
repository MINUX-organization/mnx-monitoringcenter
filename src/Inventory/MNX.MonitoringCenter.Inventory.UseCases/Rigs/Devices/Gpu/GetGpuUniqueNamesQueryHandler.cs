using MediatR;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Gpu;

namespace MNX.MonitoringCenter.Inventory.UseCases.RigInventory.Devices.Gpu;

/// <summary>
/// Обработчик <see cref="GetGpuUniqueNamesQuery"/>.
/// </summary>
public class GetGpuUniqueNamesQueryHandler : IStreamRequestHandler<GetGpuUniqueNamesQuery, string>
{
    private readonly IGpuRepository _repository;

    public GetGpuUniqueNamesQueryHandler(IGpuRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public IAsyncEnumerable<string> Handle(GetGpuUniqueNamesQuery request, CancellationToken cancellationToken)
    {
        return _repository.GetGpusUniqueNames(request.Specification);
    }
}
