using MediatR;
using MNX.Application.UseCases.CommandValidation;

namespace MNX.MonitoringCenter.Management.Agent.Commands.Mining;

/// <summary>
/// Команда установки пользовательского майнера.
/// </summary>
/// <param name="Name"> Наименование майнера. </param>
/// <param name="Version"> Версия майнера. </param>
/// <param name="InstallationUrl"> Установочная ссылка. </param>
/// <param name="PoolTemplate"> Шаблон блока пула. </param>
/// <param name="WalletWorkerTemplate"> Адрес кошелька и имя воркера для идентификации на пуле. </param>
public sealed record InstallCustomMinerCommand(
    string Name,
    string Version,
    string InstallationUrl,
    string PoolTemplate,
    string WalletWorkerTemplate)
    : IValidatableCommand<Unit>;