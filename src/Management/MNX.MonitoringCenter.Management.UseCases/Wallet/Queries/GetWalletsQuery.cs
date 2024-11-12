using AutoMapper;
using MediatR;
using MNX.MonitoringCenter.Management.Contracts;
using System.Runtime.CompilerServices;

namespace MNX.MonitoringCenter.Management.UseCases.Wallet.Queries;

/// <summary>
/// Модель запроса списка кошельков
/// </summary>
public sealed record GetWalletsQuery : IStreamRequest<WalletModel>
{
    /// <summary>
    /// Спецификация.
    /// </summary>
    public Specification Specification { get; }

    public GetWalletsQuery(Guid userId)
    {
        Specification = new Specification(userId);
    }

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

    private readonly IMapper _mapper;

    public GetWalletsQueryHandler(IWalletRepository repository, IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async IAsyncEnumerable<WalletModel> Handle(GetWalletsQuery request,
                                                     [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach (var wallet in _repository.GetAllAvailable(request.Specification).WithCancellation(cancellationToken))
        {
            yield return _mapper.Map<WalletModel>(wallet);
        }
    }
}
