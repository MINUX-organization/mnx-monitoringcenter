using AutoMapper;
using MediatR;
using MNX.Application.UseCases.CommandValidation;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Cpu;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.MiningDevice.Commands.SetRigDevices;

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

    private readonly IMiningDeviceRepository _repository;

    public SetRigsDevicesCommandHandler(IMapper mapper, IMiningDeviceRepository repository)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<Result<Unit>> Handle(SetRigDevicesCommand request, CancellationToken cancellationToken)
    {
        var miningDevices = request.Gpus.Select(gpu =>
        {
            var device = new Core.Mining.MiningDevice.MiningDevice()
            {
                Id = gpu.Id,
                Manufacturer = gpu.Information.Manufacturer,
                Model = gpu.Information.Model,
                OwnerId = request.RigOwnerId,
                Type = MiningDeviceType.GPU
            };

            device.SetOverclocking(_mapper.Map<Core.Overclocking.GpuOverclocking>(gpu.Overclocking));

            return device;
        })
        .ToList();

        miningDevices.AddRange(request.Cpus.Select(cpu =>
        {
            var device = new Core.Mining.MiningDevice.MiningDevice()
            {
                Id = cpu.Id,
                Manufacturer = cpu.Information.Manufacturer,
                Model = cpu.Information.Model,
                OwnerId = request.RigOwnerId,
                Type = MiningDeviceType.CPU
            };

            device.SetOverclocking(_mapper.Map<Core.Overclocking.CpuOverclocking>(cpu.Overclocking));

            return device;
        }));

        await _repository.SetCurrentRigDevices(request.RigId, miningDevices);

        return Result<Unit>.Empty();
    }
}
