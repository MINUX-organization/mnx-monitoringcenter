using Microsoft.Extensions.Caching.Memory;
using MINUX.Backend.Worker.Core.HardwareParameters;
using MINUX.Backend.Worker.Core.HardwareParameters.Cpu;
using MINUX.Backend.Worker.Core.HardwareParameters.Gpu;
using MINUX.Backend.Worker.Core.HardwareParameters.Harddrive;
using MINUX.Backend.Worker.Core.HardwareParameters.Motherboard;
using MINUX.Backend.Worker.UseCases.Abstractions;

namespace MINUX.Backend.Worker.DataAccess.Repositories;

public class HardwareParametersRepository //: IHardwareParametersRepository
{
    private readonly IMemoryCache _cache;

    public HardwareParametersRepository(IMemoryCache cache)
    {
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
    }

    public List<Cpu> GetCpusParameters()
    {
        return (List<Cpu>)_cache.Get("cpu");
    }

    public List<Gpu> GetGpusParameters()
    {
        return (List<Gpu>)_cache.Get("gpu");
    }

    public List<Harddrive> GetHarddrivesParameters()
    {
        return (List<Harddrive>)_cache.Get("harddrive");
    }

    public Motherboard GetMotherboardParameters()
    {
        return (Motherboard)_cache.Get("motherboard");
    }

    public List<Ram> GetRamsParameters()
    {
        return (List<Ram>)_cache.Get("ram");
    }

    public SystemInfo GetSystemInfo()
    {
        return (SystemInfo)_cache.Get("systemInfo");
    }
}
