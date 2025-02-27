using MNX.Application.UseCases.CommandValidation;
using MNX.MonitoringCenter.Management.Contracts.AlgorithmBinding;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Algorithm.Commands.AddAlgorithmCommand;

using Algorithm = Core.Mining.Algorithm;

/// <summary>
/// Команда добавления пользовательского алгоритма.
/// </summary>
public record AddAlgorithmCommand : IValidatableCommand<Algorithm>
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; }

    /// <summary>
    /// Модель алгоритма.
    /// </summary>
    public AlgorithmBindingModel Model { get; }

    public AddAlgorithmCommand(Guid userId, AlgorithmBindingModel model)
    {
        UserId = userId;
        Model = model;
    }
}