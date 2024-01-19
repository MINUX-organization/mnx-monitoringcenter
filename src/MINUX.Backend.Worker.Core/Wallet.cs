namespace MINUX.Backend.Worker.Core;

public class Wallet
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Source { get; set; }

    public string Address { get; set; }

    public Guid CryptocurrencyId { get; set; }
}