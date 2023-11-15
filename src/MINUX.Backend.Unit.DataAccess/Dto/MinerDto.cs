namespace MINUX.Backend.Unit.DataAccess.Dto;

public class MinerDto
{
    public string Name { get; set; }

    public List<MinerAlgorithm> Algorithms { get; set; } = new();
}