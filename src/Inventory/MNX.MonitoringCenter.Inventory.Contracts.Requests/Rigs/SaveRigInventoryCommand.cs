using MediatR;
using MNX.Application.UseCases.CommandValidation;
using MNX.MonitoringCenter.Common.AgentMessages;

namespace MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs;

/// <summary>
/// Команда сохранения инвентаризации рига.
/// </summary>
/// <param name="Message"> Сообщение с инвентаризацией. </param>
public sealed record SaveRigInventoryCommand(RigInventoryMsg Message) : IValidatableCommand<Unit>;
