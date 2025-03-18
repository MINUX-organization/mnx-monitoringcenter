using MediatR;
using MNX.Application.UseCases.CommandValidation;

namespace MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs;

/// <summary>
/// Команда сохранения инвентаризации рига.
/// </summary>
/// <param name="Message"> Сообщение с инвентаризацией. </param>
public sealed record SaveRigInventoryCommand(RigInventoryMsg Message) : IValidatableCommand<Unit>;
