using MediatR;

namespace MNX.MonitoringCenter.Inventory.UseCases.Gpu;

using Gpu = Contracts.Gpu.Gpu;

/// <summary>
/// Запрос на получение списка видеокарт.
/// </summary>
public class GetGpusInfoQuery : IStreamRequest<Gpu>
{
    /// <summary>
    /// Спецификация устройств.
    /// </summary>
    public DeviceSpecification Specification { get; }

    /// <summary>
    /// Создаёт экземпляр класса <see cref="GetGpusInfoQuery"/>.
    /// </summary>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <param name="rigsIds"> Идентификаторы запрашиваемых ригов. Если нет, то все доступные риги. </param>
    /// <param name="models"> Запрашиваемые модели видеокарт. Если нет, то все доступные модели. </param>
    /// <param name="manufacturers"> Запрашиваемый производители видеокарт. Если нет, то все доступные производители. </param>
    public GetGpusInfoQuery(Guid userId, Guid[]? rigsIds, string[]? models, string[]? manufacturers)
    {
        Specification = new(userId, rigsIds, models, manufacturers);
    }

    /// <summary>
    /// Создаёт экземпляр класса <see cref="GetGpusInfoQuery"/>.
    /// </summary>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <param name="rigId"> Идентификатор рига. </param>
    /// <param name="models"> Запрашиваемые модели процессоров. Если нет, то все доступные модели. </param>
    /// <param name="manufacturers"> Запрашиваемый производители процессоров. Если нет, то все доступные производители. </param>
    public GetGpusInfoQuery(Guid userId, Guid rigId, string[]? models, string[]? manufacturers)
    {
        Specification = new(userId, new Guid[] { rigId }, models, manufacturers);
    }
}

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
