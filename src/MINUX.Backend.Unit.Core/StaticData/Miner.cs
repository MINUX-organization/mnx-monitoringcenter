namespace MINUX.Backend.Unit.Core.StaticData;

public class Miner
{
    public Guid Id { get; set; }

    public string ShortName { get; set; }

    public string FullName { get; set; }

    public List<Algorithm> Algorithms { get; set; } = new();
}