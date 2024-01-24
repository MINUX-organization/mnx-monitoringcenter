using Kernel.UseCases;
using MediatR;
using MINUX.Backend.Worker.Core.HardwareParameters.Motherboard;

namespace MINUX.Backend.Worker.UseCases.Queries.GetMotherboardDataQuery;
public class GetMotherboardDataQueryHandler : IRequestHandler<GetMotherboardDataQuery, Result<Motherboard>>
{
    public Task<Result<Motherboard>> Handle(GetMotherboardDataQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
