using MediatR;
using MNX.Application.UseCases.CommandValidation;
using MNX.MonitoringCenter.Management.Core.Mining.Miner.Enums;

namespace MNX.MonitoringCenter.Management.Agent.Commands.Mining;

/// <summary>
/// Команда установки кастомного майнера на риг.
/// </summary>
/// <param name="Name"> Наименование майнера. </param>
/// <param name="Version"> Версия майнера. </param>
/// <param name="InstallationUrl"> Установочная ссылка. </param>
/// <param name="PoolTemplate"> Шаблон блока пула. </param>
/// <param name="WalletWorkerTemplate">
/// Адрес кошелька и имя воркера для идентификации на пуле.
/// </param>
public sealed record InstallMinerCommand(
    string Name,
    string Version,
    string InstallationUrl,
    string? PoolTemplate,
    string? WalletWorkerTemplate,
    MinerTypeEnum MinerType)
    : IValidatableCommand<Unit>;