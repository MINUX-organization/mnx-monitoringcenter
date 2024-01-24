using Kernel.UseCases;
using MediatR;
using MINUX.Backend.Worker.Core.HardwareParameters.Motherboard;

namespace MINUX.Backend.Worker.UseCases.Queries.GetMotherboardDataQuery;

public class GetMotherboardDataQuery : IRequest<Result<Motherboard>>
{
}
