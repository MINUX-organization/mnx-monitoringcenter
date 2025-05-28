using MNX.MonitoringCenter.Inventory.Contracts;
using MNX.Application.UseCases.CommandValidation;

namespace MNX.MonitoringCenter.Management.Agent.Commands;

/// <summary>
/// Команда создания инвентаризации.
/// </summary>
public sealed record CreateInventoryCommand : IValidatableCommand<RigInventoryModel>;
