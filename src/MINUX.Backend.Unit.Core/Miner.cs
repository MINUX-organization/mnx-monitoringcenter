namespace MINUX.Backend.Unit.Core;

public class Miner
{
    public string Name { get; set; }

    public List<Algorithm> Algorithms { get; set; } = new();
}