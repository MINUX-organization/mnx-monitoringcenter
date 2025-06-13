using MNX.Application.UseCases.CommandValidation;

namespace MNX.MonitoringCenter.Management.Agent.Commands.Mining;

/// <summary>
/// Команда остановки майнига.
/// </summary>
public sealed record StopMiningCommand : IValidatableCommand<StopMiningCommandResult>;

/// <summary>
/// Результат команды остановки майнинга.
/// </summary>
/// <param name="RigId"> Идентификатор рига. </param>
/// <param name="IsSuccess"> Признак успешности. </param>
public sealed record StopMiningCommandResult(Guid RigId, bool IsSuccess);
