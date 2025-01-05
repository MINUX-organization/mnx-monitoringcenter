using MediatR;
using MNX.Application.UseCases.CommandValidation;

namespace MNX.MonitoringCenter.Management.Agent.Commands;

/// <summary>
/// Команда выключения.
/// </summary>
public sealed record PowerOffCommand : IValidatableCommand<Unit>;
