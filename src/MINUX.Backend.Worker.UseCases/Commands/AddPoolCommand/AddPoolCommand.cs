using Kernel.UseCases;
using MediatR;

namespace MINUX.Backend.Worker.UseCases.Commands.AddPoolCommand;

/// <summary>
/// Команда добавления пула
/// </summary>
public class AddPoolCommand : IRequest<Result<Guid>>
{
    /// <summary>
    /// Хост
    /// </summary>
    public string Host { get; }

    /// <summary>
    /// Порт
    /// </summary>
    public int Port { get; }

    /// <summary>
    /// Идентификатор криптовалюты
    /// </summary>
    public Guid CryptocurrencyId { get; }

    public AddPoolCommand(string host, int port, Guid cryptocurrencyId)
    {
        Host = host;
        Port = port;
        CryptocurrencyId = cryptocurrencyId;
    }
}