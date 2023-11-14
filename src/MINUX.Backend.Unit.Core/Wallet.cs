using MINUX.Backend.Unit.Core.StaticData;

namespace MINUX.Backend.Unit.Core;

public class Wallet
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Source { get; set; }

    public string Address { get; set; }

    public Guid CryptocurrencyId { get; set; }

    public Cryptocurrency Cryptocurrency { get; set; }
}