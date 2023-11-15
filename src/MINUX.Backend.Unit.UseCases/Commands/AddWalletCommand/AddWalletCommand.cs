using Kernel.UseCases;
using MediatR;

namespace MINUX.Backend.Unit.UseCases.Commands.AddWalletCommand;

public class AddWalletCommand : IRequest<Result<Guid>>
{
    public string Name { get; set; }

    public string Source { get; set; }

    public string Address { get; set; }

    public Guid CryptocurrencyId { get; set; }

    public AddWalletCommand(string name, string source, string address, Guid cryptocurrencyId)
    {
        Name = name;
        Source = source;
        Address = address;
        CryptocurrencyId = cryptocurrencyId;
    }
}
