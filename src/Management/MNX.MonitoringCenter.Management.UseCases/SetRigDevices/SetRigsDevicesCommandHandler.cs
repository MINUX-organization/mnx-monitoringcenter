using MediatR;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;
using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.Core.Overclocking.Gpu;
using MNX.MonitoringCenter.Management.Core.Overclocking.Gpu.Fan;

namespace MNX.MonitoringCenter.Management.UseCases.SetRigDevices;

using OverclockingInventory = Inventory.Contracts.Devices.Overclocking;

/// <summary>
/// Обработчик <see cref="SetRigDevicesCommand"/>.
/// </summary>
public class SetRigsDevicesCommandHandler : IRequestHandler<SetRigDevicesCommand, Result<Unit>>
{
    private readonly IOverclockingInventoryMapper<OverclockingInventory, IOverclocking> _overclockingMapper;

    private readonly IMediator _mediator;

    private readonly IRigRepository _repository;

    ///
    public SetRigsDevicesCommandHandler(IMediator mediator,
                                        IRigRepository repository,
                                        IOverclockingInventoryMapper<OverclockingInventory, IOverclocking> overclockingMapper)
    {
        _overclockingMapper = overclockingMapper ?? throw new ArgumentNullException(nameof(overclockingMapper));
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
            var overclocking = _overclockingMapper.MapToCoreEntity(gpu.Overclocking);
            overclocking = SetFanOverclocking(overclocking, gpu.Overclocking.FanSpeed);

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
            var overclocking = _overclockingMapper.MapToCoreEntity(cpu.Overclocking);
            
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

    private static IOverclocking SetFanOverclocking(IOverclocking overclocking, int? fanSpeed)
    {
        if (overclocking is null || !fanSpeed.HasValue)
            return overclocking;

        var fanOverclocking = new FanOverclockingWithTargetSpeed { TargetSpeed = fanSpeed.Value };

        switch (overclocking)
        {
            case AmdGpuOverclocking amd:
                amd.FanOverclocking = fanOverclocking;
                break;

            case NvidiaGpuOverclocking nvidia:
                nvidia.FanOverclocking = fanOverclocking;
                break;
        }

        return overclocking;
    }
}
