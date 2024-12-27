using MNX.Application.UseCases.CommandValidation;
using MNX.MonitoringCenter.Management.Agent.Commands.Mining.ApplySettings.Models;

namespace MNX.MonitoringCenter.Management.Agent.Commands.Mining.ApplySettings;

/// <summary>
/// Команда применения настроек для воркеров.
/// </summary>
/// <param name="SettingsToApply"> Настройки для применения. </param>
public record ApplyWorkerSettingsCommand(List<WorkerSettings> SettingsToApply)
    : IValidatableCommand<ApplyWorkerSettingsCommandResult>;


/// <summary>
/// Результат команды применения настроек для воркеров.
/// </summary>
/// <param name="SuccessfullyWorkersIds">
/// Идентификаторы воркеров, на которые были успешно применены настройки.
/// </param>
/// <param name="UnsuccessfullyWorkersIds">
/// Идентификаторы воркеров, на которые не удалось применить настройки.
/// </param>
public record ApplyWorkerSettingsCommandResult(
    List<Guid> SuccessfullyWorkersIds,
    List<Guid> UnsuccessfullyWorkersIds);
