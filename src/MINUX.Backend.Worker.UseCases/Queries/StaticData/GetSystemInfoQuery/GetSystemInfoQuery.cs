using Kernel.UseCases;
using MediatR;
using MINUX.Backend.Worker.Core.StaticData;

namespace MINUX.Backend.Worker.UseCases.Queries.GetSystemInfoQuery;

public class GetSystemInfoQuery : IRequest<Result<SystemInfo>>
{
}
