using AutoMapper;
using MNX.MonitoringCenter.Management.Contracts.Miner;
using MNX.MonitoringCenter.Management.Core.Mining.Miner;
using MNX.MonitoringCenter.Management.Core.Mining.Miner.Enums;
using MNX.MonitoringCenter.Management.UseCases.Mining.Miner.Commands.Models;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping;

/// <summary>
/// Профиль маппинга для сущностей, связанных с сущностью <see cref="Miner"/>.
/// </summary>
public class MinerMappingProfile : Profile
{
    public MinerMappingProfile()
    {
        CreateMap<MinerInputModel, Miner>()
            .ForMember(miner => miner.Type, options => options.MapFrom(_ => MinerTypeEnum.Custom));
        CreateMap<Miner, MinerModel>();
        CreateMap<MinerAlgorithm, MinerAlgorithmModel>()
            .ConstructUsing(algo => new MinerAlgorithmModel(algo.AlgorithmId, algo.Name));
    }
}
