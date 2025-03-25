using MediatR;
using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet;

namespace MNX.MonitoringCenter.Management.UseCases.Mining;

/// <summary>
/// Запрос на получение майнинг комбинаций (полётный лист + майнер + монета)
/// </summary>
/// <param name="UserId"> Идентификатор пользователя. </param>
public sealed record GetMiningCombinationsQuery(Guid UserId)
    : IRequest<Dictionary<(Guid FlightSheetId, Guid MinerId, Guid CoinId), MiningCombination>>;



/// <summary>
/// Обработчик <see cref="GetMiningCombinationsQuery"/>.
/// </summary>
public class GetMiningCombinationsQueryHandler
    : IRequestHandler<GetMiningCombinationsQuery, Dictionary<(Guid FlightSheetId, Guid MinerId, Guid CoinId), MiningCombination>>
{
    private readonly IFlightSheetRepository _repository;

    public GetMiningCombinationsQueryHandler(IFlightSheetRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<Dictionary<(Guid FlightSheetId, Guid MinerId, Guid CoinId), MiningCombination>> Handle(
        GetMiningCombinationsQuery request, CancellationToken cancellationToken)
    {
        Dictionary<(Guid FlightSheetId, Guid MinerId, Guid CoinId), MiningCombination> combinations = new();

        var flightSheets = _repository.GetAllAvailable(new Specification(request.UserId));

        await foreach(var flightSheet in flightSheets.WithCancellation(cancellationToken))
        {
            flightSheet.Targets.ForEach(target => target.MiningConfig.CoinConfigs.ForEach(coin =>
            {
                combinations.TryAdd(new (flightSheet.Id, target.MinerId, coin.Pool!.CryptocurrencyId), new MiningCombination()
                {
                    FlightSheet = flightSheet.Name,
                    Miner = target.Miner!.Name,
                    Coin = coin.Pool!.Cryptocurrency!.FullName
                });
            }));
        }

        return combinations;
    }
}