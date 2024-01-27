using Kernel.UseCases;
using MediatR;

namespace MINUX.Backend.Worker.UseCases.Commands.AddCryptocurrencyCommand;

/// <summary>
/// Команда добавления криптовалюты
/// </summary>
public class AddCryptocurrencyCommand : IRequest<Result<Unit>>
{
    /// <summary>
    /// Короткое название
    /// </summary>
    public string ShortName { get; }

    /// <summary>
    /// Полное название
    /// </summary>
    public string FullName { get; }

    /// <summary>
    /// Используемый алгоритм
    /// </summary>
    public string Algorithm { get; }

    public AddCryptocurrencyCommand(string shortName, string fullName, string algorithm)
    {
        ShortName = shortName;
        FullName = fullName;
        Algorithm = algorithm;
    }
}