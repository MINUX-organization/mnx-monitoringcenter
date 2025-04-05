using MediatR;
using AutoMapper;
using System.Runtime.CompilerServices;
using MNX.Application.UseCases.Requests;
using MNX.MonitoringCenter.Management.Contracts.MiningDevice;
using MNX.MonitoringCenter.Management.UseCases.Mining.MiningDevice;

namespace MNX.MonitoringCenter.Management.UseCases.Overclocking.Presets.Queries;

/// <summary>
/// Запрос на получение списка поддерживающихся пресетом майнинг устройств,
/// сгруппированных сначала по ригу, а затем по типу.
/// </summary>
public record GetPresetSupportedDevicesQuery : IUserableStreamRequest<MiningDeviceModel>
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get => Specification.UserId; }

    /// <summary>
    /// Идентификатор пресета.
    /// </summary>
    public Guid PresetId { get; init; }

    /// <summary>
    /// Спецификация.
    /// </summary>
    public Specification Specification { get; init; }

    public GetPresetSupportedDevicesQuery(Guid userId, Guid presetId)
    {
        PresetId = presetId;
        Specification = new Specification(userId);
    }

    public GetPresetSupportedDevicesQuery(Guid userId,
                                          Guid presetId,
                                          string filterString,
                                          object[] filterParameters)
    {
        PresetId = presetId;
        Specification = new Specification(userId, filterString, filterParameters);
    }
};

/// <summary>
/// Обработчик команды <see cref="GetPresetSupportedDevicesQuery"/>.
/// </summary>
public class GetPresetSupportedDevicesQueryHandler :
    IStreamRequestHandler<GetPresetSupportedDevicesQuery, MiningDeviceModel>
{
    private readonly IMapper _mapper;

    private readonly IPresetRepository _presetRepository;

    private readonly IMiningDeviceRepository _miningDeviceRepository;

    public GetPresetSupportedDevicesQueryHandler(IMapper mapper,
                                                 IPresetRepository presetRepository,
                                                 IMiningDeviceRepository miningDeviceRepository)
    {
        _mapper = mapper ??
            throw new ArgumentNullException(nameof(mapper));
        _presetRepository = presetRepository ??
            throw new ArgumentNullException(nameof(presetRepository));
        _miningDeviceRepository = miningDeviceRepository ??
            throw new ArgumentNullException(nameof(_miningDeviceRepository));
    }

    public async IAsyncEnumerable<MiningDeviceModel> Handle(GetPresetSupportedDevicesQuery request,
                                                            [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var preset = await _presetRepository
            .GetById(request.PresetId, request.UserId, cancellationToken);

        if (preset is not null)
        {
            var devices = _miningDeviceRepository.GetAvailable(request.Specification);

            await foreach (var device in devices.WithCancellation(cancellationToken))
            {
                if (preset.IsDeviceSupport(device.Name))
                {
                    yield return _mapper.Map<MiningDeviceModel>(device);
                }
            }
        }
    }
}