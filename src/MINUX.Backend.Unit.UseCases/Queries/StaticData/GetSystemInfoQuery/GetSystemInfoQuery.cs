using Kernel.UseCases;
using MediatR;
using MINUX.Backend.Unit.Core.StaticData;

namespace MINUX.Backend.Unit.UseCases.Queries.GetSystemInfoQuery;

public class GetSystemInfoQuery : IRequest<Result<SystemInfo>>
{
}
