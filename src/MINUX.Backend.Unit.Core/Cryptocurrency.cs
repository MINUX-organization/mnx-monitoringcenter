namespace MINUX.Backend.Unit.Core;

public class Cryptocurrency
{
    public Guid Id { get; set; }

    public string ShortName { get; set; }

    public string FullName { get; set; }

    public string AlgorithmName { get; set; }

    //public MinerAlgorithm? Algorithm { get; set; }

    public List<Wallet> Wallets { get; set; } = new();

    public List<Pool> Pools { get; set; } = new(); 
}