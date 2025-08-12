using MNX.MonitoringCenter.Management.Contracts.Miner;
using MNX.MonitoringCenter.Management.UseCases.Mining.Miner;
using MNX.MonitoringCenter.Management.UseCases.Mining.Miner.Commands.Models;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Miner;

using Miner = Core.Mining.Miner.Miner;

/// <summary>
/// Реализация <see cref="IMinerMapper"/>.
/// </summary>
public class MinerMapper : IMinerMapper
{
    /// <inheritdoc/>
    public Miner MapToCoreEntity(MinerInputModel model, Guid userId)
    {
        return new Miner()
        {
            Name = model.Name,
            Version = model.Version,
            InstallationUrl = model.InstallationUrl,
            SupportedDevices = model.SupportedDevices,
            PoolTemplate = model.PoolTemplate,
            WalletWorkerTemplate = model.WalletWorkerTemplate,
            MiningMode = model.MiningMode,
            OwnerId = userId
        };
    }

    /// <inheritdoc/>
    public MinerModel MapToModel(Miner entity)
    {

        var algorithms = new List<MinerAlgorithmModel>();
        foreach (var item in entity.SupportedAlgorithms)
        {
            var algorithm = new MinerAlgorithmModel(item.AlgorithmId, item.Name);
            algorithms.Add(algorithm);
        }

        return new MinerModel()
        {
            Id = entity.Id,
            Name = entity.Name,
            Version = entity.Version,
            SupportedDevices = entity.SupportedDevices,
            SupportedAlgorithms = algorithms,
            InstallationUrl = entity.InstallationUrl,
            MiningMode = entity.MiningMode,
            OwnerId = entity.OwnerId,
            PoolTemplate = entity.PoolTemplate,
            WalletWorkerTemplate = entity.WalletWorkerTemplate
        };
    }
}
