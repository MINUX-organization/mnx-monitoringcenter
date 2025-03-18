using MNX.MonitoringCenter.Management.Core.Mining.Miner.Configs;

namespace MNX.MonitoringCenter.Management.DataAccess.FlightSheet.Dto.Target;

/// <summary>
/// DTO таргета полётного листа для процессоров.
/// </summary>
public class CpuFlightSheetTargetDto : BaseFlightSheetTargetDto
{
    /// <summary>
    /// Страницы.
    /// </summary>
    public int? HugePages { get; set; }

    /// <summary>
    /// Кол-во потоков.
    /// </summary>
    public int? ThreadsCount { get; set; }

    /// <inheritdoc/>
    public override BaseMiningConfig CreateMiningConfig()
    {
        return new CpuMiningConfig()
        {
            CoinConfigs = this.CoinConfigs,
            AdditionalArguments = this.AdditionalArguments,
            ConfigFileContent = this.ConfigFileContent,
            HugePages = this.HugePages,
            ThreadsCount = this.ThreadsCount
        };
    }
}
