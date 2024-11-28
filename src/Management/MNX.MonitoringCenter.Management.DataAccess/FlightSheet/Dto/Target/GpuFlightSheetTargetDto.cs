using MNX.MonitoringCenter.Management.Core.Miner.Configs;

namespace MNX.MonitoringCenter.Management.DataAccess.FlightSheet.Dto.Target;

/// <summary>
/// DTO таргета полётного листа для видеокарт.
/// </summary>
public class GpuFlightSheetTargetDto : BaseFlightSheetTargetDto
{
    /// <inheritdoc/>
    public override BaseMiningConfig CreateMiningConfig()
    {
        return new GpuMiningConfig()
        {
            CoinConfigs = this.CoinConfigs,
            AdditionalArguments = this.AdditionalArguments,
            ConfigFileContent = this.ConfigFileContent
        };
    }
}
