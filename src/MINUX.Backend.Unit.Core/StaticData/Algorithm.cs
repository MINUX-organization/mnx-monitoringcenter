namespace MINUX.Backend.Unit.Core.StaticData;

public class Algorithm
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public List<Cryptocurrency> Cryptocurrencies { get; set; } = new();
}