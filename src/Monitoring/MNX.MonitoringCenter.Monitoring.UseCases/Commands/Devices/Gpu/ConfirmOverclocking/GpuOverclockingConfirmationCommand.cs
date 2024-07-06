using MediatR;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Monitoring.Contracts.Bus.Models;

namespace MNX.MonitoringCenter.Monitoring.UseCases.Commands.Devices.Gpu.ConfirmOverclocking;

/// <summary>
/// Команда подтверждения установки разгона видеокарте.
/// </summary>
public class GpuOverclockingConfirmationCommand : IValidatableCommand<Unit>
{
    /// <summary>
    /// Идентификатор видеокарты.
    /// </summary>
    public Guid CardId { get; }

    /// <summary>
    /// Разгон.
    /// </summary>
    public GpuOverclockingModel Overclocking { get; }

    public GpuOverclockingConfirmationCommand(Guid cardId, GpuOverclockingModel overclocking)
    {
        CardId = cardId;
        Overclocking = overclocking;
    }
}
