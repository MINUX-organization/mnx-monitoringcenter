using AutoMapper;
using MNX.MonitoringCenter.Management.Contracts.Miner;
using MNX.MonitoringCenter.Management.Core.Mining.Miner;
using MNX.MonitoringCenter.Management.UseCases.Mining.Miner.Commands.Models;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Miner;

using Miner = Core.Mining.Miner.Miner;

/// <summary>
/// Профиль маппинга для сущностей, связанных с сущностью <see cref="Miner"/>.
/// </summary>
public class MinerMappingProfile : Profile
{
    public MinerMappingProfile()
    {
        CreateMap<MinerInputModel, Miner>();
        CreateMap<Miner, MinerModel>();
        CreateMap<MinerAlgorithm, MinerAlgorithmModel>()
            .ConstructUsing(algo => new MinerAlgorithmModel(algo.AlgorithmId, algo.Name));
    }
}
