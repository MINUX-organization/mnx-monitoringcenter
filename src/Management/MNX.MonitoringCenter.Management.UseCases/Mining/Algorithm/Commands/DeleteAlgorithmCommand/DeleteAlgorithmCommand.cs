using MediatR;
using MNX.Application.UseCases.Results;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Algorithm.Commands.DeleteAlgorithmCommand;

/// <summary>
/// Команда удаления пользовательского алгоритма.
/// </summary>
public record DeleteAlgorithmCommand : IRequest<Result<Unit>>
{
    /// <summary>
    /// Идентификатор алгоритма.
    /// </summary>
    public Guid AlgorithmId { get; }

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; }

    public DeleteAlgorithmCommand(Guid algorithmId, Guid userId)
    {
        AlgorithmId = algorithmId;
        UserId = userId;
    }
}