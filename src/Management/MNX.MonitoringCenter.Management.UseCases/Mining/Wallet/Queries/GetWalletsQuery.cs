using MediatR;
using MNX.MonitoringCenter.Management.Contracts;
using System.Runtime.CompilerServices;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Wallet.Queries;

/// <summary>
/// Модель запроса списка кошельков
/// </summary>
public sealed record GetWalletsQuery : IStreamRequest<WalletModel>
{
    /// <summary>
    /// Спецификация.
    /// </summary>
    public Specification Specification { get; }

    ///
    public GetWalletsQuery(Guid userId)
    {
        Specification = new Specification(userId);
    }

    ///
    public GetWalletsQuery(Guid userId, string filterString, object[] filterParameters)
    {
        Specification = new Specification(userId, filterString, filterParameters);
    }
}

/// <summary>
/// Реализация <see cref="GetWalletsQuery"/>.
/// </summary>
public class GetWalletsQueryHandler : IStreamRequestHandler<GetWalletsQuery, WalletModel>
{
    private readonly IWalletRepository _repository;

    private readonly IWalletMapper _walletMapper;

    ///
    public GetWalletsQueryHandler(IWalletRepository repository, IWalletMapper walletMapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _walletMapper = walletMapper ?? throw new ArgumentNullException(nameof(walletMapper));
    }

    ///
    public async IAsyncEnumerable<WalletModel> Handle(GetWalletsQuery request,
                                                     [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var wallets = _repository.GetAllAvailable(request.Specification);

        await foreach (var wallet in wallets.WithCancellation(cancellationToken))
        {
            yield return _walletMapper.MapToModel(wallet);
        }
    }
}
