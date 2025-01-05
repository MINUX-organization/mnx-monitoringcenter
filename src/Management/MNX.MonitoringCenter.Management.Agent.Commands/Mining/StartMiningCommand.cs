using MediatR;
using MNX.Application.UseCases.CommandValidation;

namespace MNX.MonitoringCenter.Management.Agent.Commands.Mining;

/// <summary>
/// Команда запуска майнига.
/// </summary>
public sealed record StartMiningCommand : IValidatableCommand<Unit>;
