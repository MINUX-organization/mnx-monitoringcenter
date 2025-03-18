using MediatR;
using MNX.Application.UseCases.CommandValidation;

namespace MNX.MonitoringCenter.Management.Agent.Commands;

/// <summary>
/// Команда перезагрузки.
/// </summary>
public sealed record RebootCommand : IValidatableCommand<Unit>;
