using MNX.MonitoringCenter.Management.Contracts.FlightSheet;
using MNX.MonitoringCenter.Management.Core.Mining.FlightSheet.Target;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Commands.Models;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.FlightSheet;

/// <summary>
/// Разделение обязанностей с <see cref="FlightSheetMapper"/>.
/// </summary>
public partial class FlightSheetMapper
{
    /// <summary>
    /// Преобразовать коллекцию сущностей <see cref="FlightSheetTargetInputModel"/>
    /// в коллекцию сущностей <see cref="FlightSheetTarget"/>
    /// с использованием идентификатора полетного листа.
    /// </summary>
    /// <param name="models"> Модель данных. </param>
    /// <param name="flightSheetId"> Идентификатор полетного листа. </param>
    /// <returns> Новая коллекция экземпляров <see cref="FlightSheetTarget"/>. </returns>
    private List<FlightSheetTarget> MapInputTargetsToCore(List<FlightSheetTargetInputModel> models, Guid flightSheetId)
    {
        var targets = new List<FlightSheetTarget>();
        foreach (var model in models)
        {
            var miningConfig = _miningConfigMapper.MapToCoreEntity(model.MiningConfig);
            var target = new FlightSheetTarget()
            {
                FlightSheetId = flightSheetId,
                MinerId = model.MinerId,
                MiningConfig = miningConfig,
            };
            targets.Add(target);
        }

        return targets;
    }

    /// <summary>
    /// Прербразовать коллекцию <see cref="FlightSheetTarget"/>
    /// в коллекцию <see cref="FlightSheetTargetModel"/>.
    /// </summary>
    /// <param name="models"> Модель данных. </param>
    /// <returns> Новая коллекция <see cref="FlightSheetTargetModel"/>. </returns>
    private List<FlightSheetTargetModel> MapTargetsToModel(List<FlightSheetTarget> models)
    {
        var targets = new List<FlightSheetTargetModel>();

        foreach (var model in models)
        {
            var minerModel = _minerMapper.MapToModel(model.Miner!);
            var configModel = _miningConfigMapper.MapToModel(model.MiningConfig);
            var target = new FlightSheetTargetModel()
            {
                Miner = minerModel,
                MiningConfig = configModel
            };

            targets.Add(target);
        }

        return targets;
    }
}
