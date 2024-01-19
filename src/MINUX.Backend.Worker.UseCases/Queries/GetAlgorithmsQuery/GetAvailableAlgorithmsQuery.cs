using MediatR;

namespace MINUX.Backend.Worker.UseCases.Queries.GetAlgorithmsQuery;

/// <summary>
/// Запрос на получение доступных алгоритмов
/// </summary>
public sealed record GetAvailableAlgorithmsQuery() : IStreamRequest<string>;