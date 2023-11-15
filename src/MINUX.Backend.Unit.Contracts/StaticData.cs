using MINUX.Backend.Unit.Core;
using MINUX.Backend.Unit.Core.StaticData;
using MINUX.Backend.Unit.Core.StaticData.Cpu;
using MINUX.Backend.Unit.Core.StaticData.Gpu;
using MINUX.Backend.Unit.Core.StaticData.Motherboard;

namespace MINUX.Backend.Unit.Contracts;

public class StaticData
{
    public List<Gpu> Gpus { get; set; } = new();

    public Cpu Cpu { get; set; } = new();

    public List<Miner> Miners { get; set; } = new();

    public List<Ram> Rams { get; set; } = new();

    public Motherboard Motherboard { get; set; }

    public SystemInfo SystemInfo { get; set; }
}