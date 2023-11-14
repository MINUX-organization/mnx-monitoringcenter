using MINUX.Backend.Unit.Core.StaticData;

namespace MINUX.Backend.Unit.Core;

public class Cryptocurrency
{
    public string ShortName { get; set; }

    public string FullName { get; set; }

    public Guid AlgorithmId { get; set; }

    public Algorithm Algorithm { get; set; }

    public List<Wallet> Wallets { get; set; } = new();

    public List<Pool> Pools { get; set; } = new(); 
}