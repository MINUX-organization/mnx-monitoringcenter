namespace MINUX.Backend.Unit.Core.StaticData.Cpu;

// TODO: Уточнить на сёт модели
public class CpuCores
{
    public int CpuCount { get; set; }

    public int SocketCount { get; set; }

    public int ThreadsPerCoreCount { get; set; }

    public int ThreadsPerSocketCount { get; set; }
}