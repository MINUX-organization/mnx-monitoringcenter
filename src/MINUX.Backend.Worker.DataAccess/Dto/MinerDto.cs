namespace MINUX.Backend.Worker.DataAccess.Dto;

public class MinerDto
{
    public string Name { get; set; }

    public List<MinerAlgorithm> Algorithms { get; set; } = new();
}