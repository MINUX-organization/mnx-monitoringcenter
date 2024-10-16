using MediatR;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Inventory.Contracts.RigInventory;

namespace MNX.MonitoringCenter.Inventory.Contracts.Requests.Rig;

/// <summary>
/// Команда сохранения инвентаризации рига.
/// </summary>
/// <param name="Message"> Сообщение с инвентаризацией. </param>
public sealed record SaveRigInventoryCommand(RigInventoryMsg Message) : IValidatableCommand<Unit>;
