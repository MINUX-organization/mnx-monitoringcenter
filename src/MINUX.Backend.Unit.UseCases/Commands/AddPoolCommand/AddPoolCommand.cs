using Kernel.UseCases;
using MediatR;

namespace MINUX.Backend.Unit.UseCases.Commands.AddPoolCommand;

public class AddPoolCommand : IRequest<Result<Guid>>
{
    public string Host { get; }

    public int Port { get; }

    public Guid CryptocurrencyId { get; }

    public AddPoolCommand(string host, int port, Guid cryptocurrencyId)
    {
        Host = host;
        Port = port;
        CryptocurrencyId = cryptocurrencyId;
    }
}