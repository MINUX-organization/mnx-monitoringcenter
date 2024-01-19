using MINUX.Backend.Worker.Core;
using MINUX.Backend.Worker.Core.StaticData;
using MINUX.Backend.Worker.Core.StaticData.Cpu;
using MINUX.Backend.Worker.Core.StaticData.Gpu;
using MINUX.Backend.Worker.Core.StaticData.Motherboard;

namespace MINUX.Backend.Worker.Contracts;

public class StaticData
{
    public List<Gpu> Gpus { get; set; } = new();

    public Cpu Cpu { get; set; } = new();

    public List<Miner> Miners { get; set; } = new();

    public List<Ram> Rams { get; set; } = new();

    public Motherboard Motherboard { get; set; }

    public SystemInfo SystemInfo { get; set; }
}