using MediatR;
using Kernel.UseCases;
using MINUX.Backend.Unit.Contracts;

namespace MINUX.Backend.Unit.UseCases.Queries.GetStaticDataQuery;

public class GetStaticDataQuery : IRequest<Result<StaticData>>
{
}