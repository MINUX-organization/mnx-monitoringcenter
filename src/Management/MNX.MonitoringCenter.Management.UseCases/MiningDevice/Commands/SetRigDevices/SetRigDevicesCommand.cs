using MediatR;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Cpu;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu;
using MNX.MonitoringCenter.Management.Core.MiningDevice;

namespace MNX.MonitoringCenter.Management.UseCases.MiningDevice.Commands.SetRigDevices;

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
    private readonly IMiningDeviceRepository _repository;

    public SetRigsDevicesCommandHandler(IMiningDeviceRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<Result<Unit>> Handle(SetRigDevicesCommand request, CancellationToken cancellationToken)
    {
        var miningDevices = request.Gpus.Select(gpu => new Core.MiningDevice.MiningDevice()
        {
            Id = gpu.Id,
            RigId = request.RigId,
            OwnerId = request.RigOwnerId,
            Type = MiningDeviceType.GPU
        })
        .ToList();

        miningDevices.AddRange(request.Cpus.Select(cpu => new Core.MiningDevice.MiningDevice()
        {
            Id = cpu.Id,
            RigId = request.RigId,
            OwnerId = request.RigOwnerId,
            Type = MiningDeviceType.CPU
        }));

        await _repository.SetCurrentRigsDevices(miningDevices, cancellationToken);

        return Result<Unit>.Empty();
    }
}
