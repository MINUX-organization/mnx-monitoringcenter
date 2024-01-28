using Kernel.UseCases;
using MediatR;

namespace MINUX.Backend.Worker.UseCases.Commands.Crypto.RemoveCryptocurrency;

/// <summary>
/// Команда удаления криптовалюты
/// </summary>
public class RemoveCryptocurrencyCommand : IRequest<Result<Unit>>
{
    /// <summary>
    /// Полное название криптовалюты
    /// </summary>
    public string FullName { get; }

    public RemoveCryptocurrencyCommand(string fullName)
    {
        FullName = fullName;
    }
}
