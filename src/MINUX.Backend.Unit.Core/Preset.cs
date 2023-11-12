namespace MINUX.Backend.Unit.Core;

public class Preset
{
    public Guid Id { get; set; }

    public int MemoryClock { get; set; }

    public int CoreClock { get; set; }

    public int PowerLimit {  get; set; }

    public int CritTemp { get; set; }

    public int FanSpeed { get; set; }

    public int GpuId { get; set; }
}