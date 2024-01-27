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
    /// Полное название криптовалюты
    /// </summary>
    public string CryptocurrencyFullName { get; }

    public AddPoolCommand(string host, int port, string cryptocurrencyFullName)
    {
        Host = host;
        Port = port;
        CryptocurrencyFullName = cryptocurrencyFullName;
    }
}