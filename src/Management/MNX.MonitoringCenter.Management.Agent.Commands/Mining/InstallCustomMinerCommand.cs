using MediatR;
using MNX.Application.UseCases.CommandValidation;

namespace MNX.MonitoringCenter.Management.Agent.Commands.Mining;

public sealed record InstallCustomMinerCommand(
    string Name,
    Guid UserId,
    string Version,
    string InstallationUrl,
    string PoolTemplate,
    string WalletWorkerTemplate)
    : IValidatableCommand<Unit>;