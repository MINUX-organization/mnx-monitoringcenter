using AutoMapper;
using MediatR;
using MNX.Application.UseCases.CommandValidation;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Cpu;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;
using MNX.MonitoringCenter.Management.Core.Overclocking;

namespace MNX.MonitoringCenter.Management.UseCases.SetRigDevices;

/// <summary>
/// Команда на установку майнинг устройств на риг.
/// </summary>
/// <param name="RigId"> Идентификатор рига. </param>
/// <param name="RigOwnerId"> Идентификатор владельца рига. </param>
/// <param name="Gpus"> Список видеокарт. </param>
/// <param name="Cpus"> Список процессоров. </param>
public sealed record SetRigDevicesCommand(Guid RigId, Guid RigOwnerId, List<Gpu> Gpus, List<Cpu> Cpus)
    : IValidatableCommand<Unit>;


/// <summary>
/// Обработчик <see cref="SetRigDevicesCommand"/>.
/// </summary>
public class SetRigsDevicesCommandHandler : IRequestHandler<SetRigDevicesCommand, Result<Unit>>
{
    private readonly IMapper _mapper;

    private readonly IRigRepository _repository;

    public SetRigsDevicesCommandHandler(IMapper mapper, IRigRepository repository)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<Result<Unit>> Handle(SetRigDevicesCommand request, CancellationToken cancellationToken)
    {
        var devicesTupple = new List<(Core.Mining.MiningDevice.MiningDevice Devices, IOverclocking Overclockings)>();

        devicesTupple.AddRange(request.Gpus.Select(gpu =>
        {
            var overclocking = _mapper.Map<Core.Overclocking.GpuOverclocking>(gpu.Overclocking);

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
            var overclocking = _mapper.Map<Core.Overclocking.CpuOverclocking>(cpu.Overclocking);
            
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

        return Result<Unit>.Empty();
    }
}
