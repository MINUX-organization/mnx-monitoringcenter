using MediatR;
using MNX.Application.UseCases.CommandValidation;

namespace MNX.MonitoringCenter.Management.Agent.Commands.Mining;

/// <summary>
/// Команда остановки майнига.
/// </summary>
public sealed record StopMiningCommand : IValidatableCommand<Unit>;
