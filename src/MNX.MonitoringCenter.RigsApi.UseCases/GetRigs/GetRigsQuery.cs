using MediatR;
using MNX.Application.UseCases.Requests;
using MNX.MonitoringCenter.RigsApi.Core.Services;
using System.Runtime.CompilerServices;

namespace MNX.MonitoringCenter.RigsApi.UseCases.GetRigs;

/// <summary>
/// Запрос на получения списка ригов.
/// </summary>
/// <param name="UserId"> Идентификатор пользователя. </param>
public record GetRigsQuery(Guid UserId) : IUserableStreamRequest<RigModel>;


/// <summary>
/// Обработчик <see cref="GetRigsQuery"/>.
/// </summary>
public class GetRigsQueryHandler : IStreamRequestHandler<GetRigsQuery, RigModel>
{
    private readonly IRigRepository _repository;

    public GetRigsQueryHandler(IRigRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async IAsyncEnumerable<RigModel> Handle(
        GetRigsQuery request,
       [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var rigsStream = _repository.GetAvailable(request.UserId);

        await foreach (var rig in rigsStream.WithCancellation(cancellationToken))
        {
            yield return new RigModel()
            {
                Id = rig.Id,
                OwnerId = rig.OwnerId,
                Name = rig.Name,
                CurrentInventoryId = rig.CurrentInventoryId,
                IsOnline = rig.IsOnline,
                LifeCycleStatus = rig.LifeCycleStatus,
                MiningLifeCycleStatus = rig.MiningLifeCycleStatus
            };
        }
    }
}
