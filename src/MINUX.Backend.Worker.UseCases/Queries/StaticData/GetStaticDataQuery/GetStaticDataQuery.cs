using MediatR;
using Kernel.UseCases;
using MINUX.Backend.Worker.Contracts;

namespace MINUX.Backend.Worker.UseCases.Queries.GetStaticDataQuery;

public class GetStaticDataQuery : IRequest<Result<StaticData>>
{
}