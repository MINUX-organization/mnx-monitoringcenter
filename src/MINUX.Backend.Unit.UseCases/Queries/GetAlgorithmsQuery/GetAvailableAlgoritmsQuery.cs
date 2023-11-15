using MediatR;

namespace MINUX.Backend.Unit.UseCases.Queries.GetAlgorithmsQuery;

/// <summary>
/// Запрос на получение доступных алгоритмов
/// </summary>
public sealed record GetAvailableAlgoritmsQuery() : IStreamRequest<string>;