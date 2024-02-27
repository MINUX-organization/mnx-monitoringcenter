using AutoMapper;
using MediatR;
using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;
using System.Runtime.CompilerServices;

namespace MNX.MonitoringCenter.Management.UseCases.Queries.GetWalletsQuery;

public class GetWalletsQueryHandler : IStreamRequestHandler<GetWalletsQuery, WalletModel>
{
    private readonly IWalletRepository _repository;

    private readonly IMapper _mapper;

    public GetWalletsQueryHandler(IWalletRepository repository, IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(_mapper));
    }

    public async IAsyncEnumerable<WalletModel> Handle(GetWalletsQuery request,
                                                     [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach (var wallet in _repository.GetAll())
        {
            yield return _mapper.Map<WalletModel>(wallet);
        }
    }
}