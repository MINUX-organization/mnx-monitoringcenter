using MediatR;
using MNX.Application.UseCases.CommandValidation;

namespace MNX.MonitoringCenter.Management.Agent.Commands.Overclocking;

/// <summary>
/// Команда установки разгона.
/// </summary>
/// <param name="Overclocking"> Разгон. </param>
/// <param name="Workers"> Воркеры. </param>
public sealed record SetOverclockingCommand(Inventory.Contracts.Devices.Overclocking Overclocking, params Guid[] Workers)
    : IValidatableCommand<Unit>;
