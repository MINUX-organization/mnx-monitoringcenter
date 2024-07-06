using MediatR;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Monitoring.Contracts.Bus.Models;

namespace MNX.MonitoringCenter.Monitoring.UseCases.Commands.Devices.Gpu.SetOverclocking;

/// <summary>
/// Команда установки разгона видеокарте.
/// </summary>
public class SetGpuOverclockingCommand : IValidatableCommand<Unit>
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// Идентификатор соединения веб-клиента.
    /// </summary>
    public string ConnectionId { get; set; }

    /// <summary>
    /// Идентификатор видеокарты.
    /// </summary>
    public Guid CardId { get; set; }

    /// <summary>
    /// Разгон видеокарты.
    /// </summary>
    public GpuOverclockingModel Overclocking { get; set; }

    public SetGpuOverclockingCommand(long userId, string connectionId, Guid cardId, GpuOverclockingModel overclocking)
    {
        UserId = userId;
        ConnectionId = connectionId;
        CardId = cardId;
        Overclocking = overclocking;
    }
}
