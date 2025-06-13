using MNX.Application.UseCases.CommandValidation;

namespace MNX.MonitoringCenter.Management.Agent.Commands.Mining;

/// <summary>
/// Команда запуска майнига.
/// </summary>
public sealed record StartMiningCommand : IValidatableCommand<StartMiningCommandResult>;

/// <summary>
/// Результат команды запуска майнинга.
/// </summary>
/// <param name="RigId"> Идентификатор рига. </param>
/// <param name="IsSuccess"> Признак успешности. </param>
public sealed record StartMiningCommandResult(Guid RigId, bool IsSuccess);
