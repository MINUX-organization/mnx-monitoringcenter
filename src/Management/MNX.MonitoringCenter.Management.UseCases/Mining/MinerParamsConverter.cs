using MNX.MonitoringCenter.Management.Agent.Commands.Mining;
using MNX.MonitoringCenter.Management.Core.Mining.Miner.Enums;

namespace MNX.MonitoringCenter.Management.UseCases.Mining;

/// <summary>
/// Конвертер значений перечислений типов майнера.
/// </summary>
public static class MinerParamsConverter
{
    /// <summary>
    /// Конвертировать в перечисление из контрактов.
    /// </summary>
    /// <param name="model"> Перечисление из Core-слоя. </param>
    /// <returns> Перечисление из контрактов. </returns>
    public static MinerTypeEnumModel ToMinerTypeContract(this MinerTypeEnum model)
    {
        return model switch
        {
            MinerTypeEnum.Integrated => MinerTypeEnumModel.Integrated,
            MinerTypeEnum.Custom => MinerTypeEnumModel.Custom
        };
    }

    /// <summary>
    /// Конвертировать в перечисление из core-слоя.
    /// </summary>
    /// <param name="model"> Перечисление из Contract-слоя. </param>
    /// <returns> Перечисление из core-слоя. </returns>
    public static MinerTypeEnum ToMinerTypeCore(this MinerTypeEnumModel model)
    {
        return model switch
        {
            MinerTypeEnumModel.Integrated => MinerTypeEnum.Integrated,
            MinerTypeEnumModel.Custom => MinerTypeEnum.Custom
        };
    }
}
