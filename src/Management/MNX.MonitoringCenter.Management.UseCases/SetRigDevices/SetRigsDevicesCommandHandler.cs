using AutoMapper;
using MediatR;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;
using MNX.MonitoringCenter.Management.Core.Overclocking;

namespace MNX.MonitoringCenter.Management.UseCases.SetRigDevices;

/// <summary>
/// Обработчик <see cref="SetRigDevicesCommand"/>.
/// </summary>
public class SetRigsDevicesCommandHandler : IRequestHandler<SetRigDevicesCommand, Result<Unit>>
{
    private readonly IMapper _mapper;

    private readonly IMediator _mediator;

    private readonly IRigRepository _repository;

    ///
    public SetRigsDevicesCommandHandler(IMapper mapper,
                                        IMediator mediator,
                                        IRigRepository repository)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    ///
    public async Task<Result<Unit>> Handle(SetRigDevicesCommand request,
                                           CancellationToken cancellationToken)
    {
        var devicesTupple = new List<(Core.Mining.MiningDevice.MiningDevice Devices, IOverclocking Overclockings)>();

        devicesTupple.AddRange(request.Gpus.Select(gpu =>
        {
            var overclocking = _mapper.Map<IOverclocking>(gpu.Overclocking);

            var device = new Core.Mining.MiningDevice.MiningDevice()
            {
                Id = gpu.Id,
                Manufacturer = gpu.Information.Manufacturer,
                Model = gpu.Information.Model,
                OwnerId = request.RigOwnerId,
                Type = MiningDeviceType.GPU
            };

            return (device, (IOverclocking)overclocking);
        }));

        devicesTupple.AddRange(request.Cpus.Select(cpu =>
        {
            var overclocking = _mapper.Map<CpuOverclocking>(cpu.Overclocking);
            
            var device = new Core.Mining.MiningDevice.MiningDevice()
            {
                Id = cpu.Id,
                Manufacturer = cpu.Information.Manufacturer,
                Model = cpu.Information.Model,
                OwnerId = request.RigOwnerId,
                Type = MiningDeviceType.CPU
            };

            return (device, (IOverclocking)overclocking);
        }));

        await _repository.SetDevices(request.RigId, devicesTupple);

        await _mediator.Publish(new MiningDeviceStateChangedEvent(request.RigOwnerId.ToString()), cancellationToken);

        return Result<Unit>.Empty();
    }
}
