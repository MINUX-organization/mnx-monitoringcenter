using MediatR;

namespace MNX.MonitoringCenter.Management.UseCases.Queries.GetAlgorithmsQuery;

/// <summary>
/// Запрос на получение доступных алгоритмов
/// </summary>
public sealed record GetAvailableAlgorithmsQuery() : IStreamRequest<string>;