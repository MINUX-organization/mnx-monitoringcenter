using MNX.MonitoringCenter.Management.Core.HardwareParameters;

namespace MNX.MonitoringCenter.Management.Core;

/// <summary>
/// Полётный лист ( конфигурация для воркера )
/// </summary>
public class FlightSheet
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public Miner Miner { get; set; }

    public string Cryptocurrency { get; set; }

    public string WalletAddress { get; set; }

    public Pool Pool { get; set; }

    public long UserId {  get; set; }
}