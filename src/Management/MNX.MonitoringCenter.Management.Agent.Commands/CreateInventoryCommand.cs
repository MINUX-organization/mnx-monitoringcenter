using MNX.Application.UseCases.CommandValidation;
using MNX.MonitoringCenter.Inventory.Contracts;

namespace MNX.MonitoringCenter.Management.Agent.Commands;

/// <summary>
/// Команда создания инвентаризации.
/// </summary>
public sealed record CreateInventoryCommand : IValidatableCommand<RigInventoryModel>;
