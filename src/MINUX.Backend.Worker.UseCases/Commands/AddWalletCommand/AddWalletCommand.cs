using Kernel.UseCases;
using MediatR;

namespace MINUX.Backend.Worker.UseCases.Commands.AddWalletCommand;

/// <summary>
/// Команда добавления кошелька
/// </summary>
public class AddWalletCommand : IRequest<Result<Guid>>
{
    /// <summary>
    /// Название кошелька
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Ссылка на кошелёк
    /// </summary>
    public string Source { get; set; }

    /// <summary>
    /// Адрес кошелька
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// Идентификатор криптовалюты
    /// </summary>
    public Guid CryptocurrencyId { get; set; }

    public AddWalletCommand(string name, string source, string address, Guid cryptocurrencyId)
    {
        Name = name;
        Source = source;
        Address = address;
        CryptocurrencyId = cryptocurrencyId;
    }
}
