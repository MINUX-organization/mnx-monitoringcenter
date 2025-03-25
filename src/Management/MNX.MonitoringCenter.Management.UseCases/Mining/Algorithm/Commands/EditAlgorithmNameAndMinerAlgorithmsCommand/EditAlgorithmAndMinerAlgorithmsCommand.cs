using MNX.Application.UseCases.CommandValidation;
using MNX.MonitoringCenter.Management.Contracts.AlgorithmBinding;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Algorithm.Commands.EditAlgorithmNameAndMinerAlgorithmsCommand;

using Algorithm = Core.Mining.Algorithm;

/// <summary>
/// Команда редактирования пользовательского 
/// алгоритма и относительных наименований алгоритма.
/// </summary>
public class EditAlgorithmAndMinerAlgorithmsCommand : IValidatableCommand<Algorithm>
{
    /// <summary>
    /// Идентификатор алгоритма.
    /// </summary>
    public Guid AlgorithmId { get; }

    /// <summary>
    /// Пользовательский идентификатор.
    /// </summary>
    public Guid UserId { get; }

    /// <summary>
    /// Модель пользовательского алгоритма.
    /// </summary>
    public AlgorithmBindingModel Model { get; }

    public EditAlgorithmAndMinerAlgorithmsCommand(Guid algorithmId,
                                                  Guid userId,
                                                  AlgorithmBindingModel model)
    {
        AlgorithmId = algorithmId;
        UserId = userId;
        Model = model;
    }
};
