using AutoMapper;
using MediatR;
using MNX.MonitoringCenter.Monitoring.Core.Devices;
using MNX.MonitoringCenter.Monitoring.UseCases.Abstractions;
using System.Runtime.CompilerServices;

namespace MNX.MonitoringCenter.Monitoring.UseCases.Queries.Devices.GetGpusInfo;

/// <summary>
/// Обработчик запроса на получение информации о видеокартах.
/// </summary>
public class GetGpusInfoQueryHandler : IStreamRequestHandler<GetGpusInfoQuery, GpuInfo>
{
    /// <summary>
    /// Репозиторий для майнинг устройств.
    /// </summary>
    private readonly IMiningDeviceRepository _deviceRepository;

    /// <summary>
    /// Автомаппер.
    /// </summary>
    private readonly IMapper _mapper;

    public GetGpusInfoQueryHandler(IMiningDeviceRepository deviceRepository, IMapper mapper)
    {
        _deviceRepository = deviceRepository ?? throw new ArgumentNullException(nameof(deviceRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(_mapper));
    }

    public async IAsyncEnumerable<GpuInfo> Handle(GetGpusInfoQuery request,
                                                 [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach (var device in _deviceRepository.GetList(request.Specification))
        {
            yield return _mapper.Map<GpuInfo>(device as Gpu);
        }
    }
}
