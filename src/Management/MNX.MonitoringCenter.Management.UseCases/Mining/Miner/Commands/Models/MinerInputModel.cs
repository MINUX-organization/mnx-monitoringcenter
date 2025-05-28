using MNX.MonitoringCenter.Management.Core.Mining.Miner.Enums;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Miner.Commands.Models;

/// <summary>
/// Модель ввода пользовательского майнера.
/// </summary>
/// <param name="Name"> Наименование майнера. </param>
/// <param name="Version"> Версия майнера. </param>
/// <param name="InstallationUrl"> Установочная ссылка. </param>
/// <param name="PoolTemplate"> Шаблон блока пула. </param>
/// <param name="WalletWorkerTemplate"> Адрес кошелька и имя воркера для идентификации на пуле. </param>
/// <param name="MiningMode"> Режим майнинга. </param>
public record MinerInputModel(
    string Name,
    string Version,
    string InstallationUrl,
    DeviceTypeManufacturerCombination SupportedDevices,
    string PoolTemplate,
    string WalletWorkerTemplate,
    MiningModeEnum MiningMode);
