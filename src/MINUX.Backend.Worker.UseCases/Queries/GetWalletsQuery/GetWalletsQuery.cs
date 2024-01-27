using MediatR;
using MINUX.Backend.Worker.Core;

namespace MINUX.Backend.Worker.UseCases.Queries.GetWalletsQuery;

/// <summary>
/// Модель запроса списка кошельков
/// </summary>
public class GetWalletsQuery : IStreamRequest<Wallet>
{
}