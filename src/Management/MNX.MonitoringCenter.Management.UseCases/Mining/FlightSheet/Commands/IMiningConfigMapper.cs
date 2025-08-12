using MNX.MonitoringCenter.Management.Contracts.FlightSheet.MiningConfigs;
using MNX.MonitoringCenter.Management.Core.Mining.Miner.Configs;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Commands.Models.MiningConfig;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Commands;

/// <summary>
/// Интерфейс маппера реализаций сущности
/// <see cref="BaseMiningConfig"/> и её моделей. 
/// </summary>
public interface IMiningConfigMapper
{
    /// <summary>
    /// Преобразовать сущность <see cref="MiningConfigInputModel"/>
    /// в <see cref="BaseMiningConfig"/>.
    /// </summary>
    /// <param name="model"> Модель данных. </param>
    /// <returns>
    /// Новый экземпляр сущности <see cref="BaseMiningConfig"/>.
    /// </returns>
    BaseMiningConfig MapToCoreEntity(MiningConfigInputModel model);

    /// <summary>
    /// Преобразовать реализацию сущности <see cref="BaseMiningConfig"/>
    /// в реализацию модели <see cref="BaseMiningConfigModel"/>
    /// </summary>
    /// <param name="model"> Модель данных. </param>
    /// <returns> Новый экземпляр реализации <see cref="BaseMiningConfigModel"/>. </returns>
    /// <exception cref="NotImplementedException">
    /// Выбрасывается в случае получение некорректного типа девайса в конфигурации майнинга.
    /// </exception>
    public BaseMiningConfigModel MapToModel(BaseMiningConfig model);
}
