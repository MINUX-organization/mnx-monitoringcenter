using AutoMapper;
using MediatR;
using MNX.MonitoringCenter.Monitoring.UseCases.Abstractions;
using System.Runtime.CompilerServices;

namespace MNX.MonitoringCenter.Monitoring.UseCases.Queries.GetRigsInformation;

/// <summary>
/// Обработчик запроса на получение информации о ригах
/// </summary>
public class GetRigsInformationQueryHandler : IStreamRequestHandler<GetRigsInformationQuery, RigInformationMessage>
{
    /// <summary>
    /// Маппер.
    /// </summary>
    private readonly IMapper _mapper;

    /// <summary>
    /// Репозиторий для доступа к ригам.
    /// </summary>
    private readonly IRigRepository _rigRepository;

    public GetRigsInformationQueryHandler(IMapper mapper, IRigRepository rigRepository)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _rigRepository = rigRepository ?? throw new ArgumentNullException(nameof(rigRepository));
    }

    public async IAsyncEnumerable<RigInformationMessage> Handle(GetRigsInformationQuery request,
                                                                [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var counter = 0;

        await foreach (var rig in _rigRepository.GetList(request.Specification))
        {
            counter++;
            var rigInformation = _mapper.Map<RigInformationMessage>(rig);
            rigInformation.Index = counter;

            yield return rigInformation;
        }
    }
}
