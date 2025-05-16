using MediatR;
using MNX.Application.UseCases.CommandValidation;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Cpu;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu;

namespace MNX.MonitoringCenter.Management.UseCases.SetRigDevices;

/// <summary>
/// Команда на установку майнинг устройств на риг.
/// </summary>
/// <param name="RigId"> Идентификатор рига. </param>
/// <param name="RigOwnerId"> Идентификатор владельца рига. </param>
/// <param name="Gpus"> Список видеокарт. </param>
/// <param name="Cpus"> Список процессоров. </param>
public sealed record SetRigDevicesCommand(Guid RigId,
                                          Guid RigOwnerId,
                                          List<Gpu> Gpus,
                                          List<Cpu> Cpus)
    : IValidatableCommand<Unit>;
