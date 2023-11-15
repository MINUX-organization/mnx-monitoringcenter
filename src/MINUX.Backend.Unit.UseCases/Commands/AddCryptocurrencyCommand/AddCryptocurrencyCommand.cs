using Kernel.UseCases;
using MediatR;

namespace MINUX.Backend.Unit.UseCases.Commands.AddCryptocurrencyCommand;

public class AddCryptocurrencyCommand : IRequest<Result<Guid>>
{
    public string ShortName { get; }

    public string FullName { get; }

    public string Algorithm { get; }

    public AddCryptocurrencyCommand(string shortName, string fullName, string algorithm)
    {
        ShortName = shortName;
        FullName = fullName;
        Algorithm = algorithm;
    }
}