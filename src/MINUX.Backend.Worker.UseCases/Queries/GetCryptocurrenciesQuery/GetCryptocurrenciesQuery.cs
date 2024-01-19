using MediatR;
using MINUX.Backend.Worker.Core;

namespace MINUX.Backend.Worker.UseCases.Queries.GetCryptocurrenciesQuery;

public class GetCryptocurrenciesQuery : IStreamRequest<Cryptocurrency>
{
}
