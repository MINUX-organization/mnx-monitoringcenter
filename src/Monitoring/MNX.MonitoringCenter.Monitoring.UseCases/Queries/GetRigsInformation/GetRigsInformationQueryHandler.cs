using AutoMapper;
using MediatR;
using MNX.MonitoringCenter.Monitoring.Core;
using MNX.MonitoringCenter.Monitoring.UseCases.Abstractions;

namespace MNX.MonitoringCenter.Monitoring.UseCases.Queries.GetRigsInformation;

/// <summary>
/// Обработчик запроса на получение информации о ригах
/// </summary>
public class GetRigsInformationQueryHandler : IRequestHandler<GetRigsInformationQuery, GetRigsInformationResult>
{
    private readonly IMapper _mapper;

    private readonly IRigRepository _rigRepository;

    public GetRigsInformationQueryHandler(IMapper mapper, IRigRepository rigRepository)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _rigRepository = rigRepository ?? throw new ArgumentNullException(nameof(rigRepository));
    }

    public async Task<GetRigsInformationResult> Handle(GetRigsInformationQuery request, CancellationToken cancellationToken)
    {
        var rigs = await _rigRepository.GetAvailable(request.specification);
        return ToResult(rigs);
    }

    /// <summary>
    /// Собрать результат подписки клиента.
    /// </summary>
    /// <param name="rigs"> Коллекция ригов. </param>
    /// <returns> Результат подписки клиента. </returns>
    private GetRigsInformationResult ToResult(IEnumerable<Rig> rigs)
    {
        var rigsInformation = _mapper.Map<IEnumerable<RigInformationMessage>>(rigs);

        for (int i = 0; i < rigsInformation.Count(); i++)
        {
            rigsInformation.ElementAt(i).Index = i + 1;
        }

        return new GetRigsInformationResult()
        {
            Rigs = rigsInformation,

            TotalCpusCount = new TotalCpusCount()
            {
                Total = rigs.Sum(x => x.TotalCpusCount),
                Amd = rigs.Sum(x => x.AmdCpusCount),
                Intel = rigs.Sum(x => x.IntelCpusCount)
            },

            TotalGpusCount = new TotalGpusCount()
            {
                Total = rigs.Sum(x => x.TotalGpusCount),
                Nvidia = rigs.Sum(x => x.NvidiaGpusCount),
                Amd = rigs.Sum(x => x.AmdGpusCount),
                Intel = rigs.Sum(x => x.IntelGpusCount)
            }
        };
    }
}
