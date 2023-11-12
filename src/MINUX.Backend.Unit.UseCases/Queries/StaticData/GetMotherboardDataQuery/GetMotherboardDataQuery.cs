using Kernel.UseCases;
using MediatR;
using MINUX.Backend.Unit.Core.StaticData.Motherboard;

namespace MINUX.Backend.Unit.UseCases.Queries.GetMotherboardDataQuery;

public class GetMotherboardDataQuery : IRequest<Result<Motherboard>>
{
}
