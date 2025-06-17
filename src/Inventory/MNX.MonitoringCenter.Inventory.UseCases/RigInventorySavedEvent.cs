using MediatR;
using MNX.MonitoringCenter.Inventory.Contracts;

namespace MNX.MonitoringCenter.Inventory.UseCases;

/// <summary>
/// Событие об успешном сохранении инвентаризации в базе данных.
/// </summary>
/// <param name="Message"> Сообщение с инвентаризацией. </param>
public sealed record RigInventorySavedEvent(RigInventoryMsg Message, long RigInventoryId) : INotification;
