using Kernel.UseCases;
using MediatR;
using MINUX.Backend.Unit.Core.StaticData.Motherboard;

namespace MINUX.Backend.Unit.UseCases.Queries.GetMotherboardDataQuery;
public class GetMotherboardDataQueryHandler : IRequestHandler<GetMotherboardDataQuery, Result<Motherboard>>
{
    public Task<Result<Motherboard>> Handle(GetMotherboardDataQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
