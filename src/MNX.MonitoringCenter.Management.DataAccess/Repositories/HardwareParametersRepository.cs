using Microsoft.Extensions.Caching.Memory;
using MNX.MonitoringCenter.Management.Core.HardwareParameters;
using MNX.MonitoringCenter.Management.Core.HardwareParameters.Cpu;
using MNX.MonitoringCenter.Management.Core.HardwareParameters.Gpu;
using MNX.MonitoringCenter.Management.Core.HardwareParameters.Harddrive;
using MNX.MonitoringCenter.Management.Core.HardwareParameters.Motherboard;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;

namespace MNX.MonitoringCenter.Management.DataAccess.Repositories;

public class HardwareParametersRepository : IHardwareParametersRepository
{
    private readonly IMemoryCache _cache;

    public HardwareParametersRepository(IMemoryCache cache)
    {
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
    }

    public Task<List<Cpu>> GetCpusParameters()
    {
        return Task.Run(() => (List<Cpu>)_cache.Get("cpu"));
    }

    public Task<List<Gpu>> GetGpusParameters()
    {
        return Task.Run(() => (List<Gpu>)_cache.Get("gpu"));
    }

    public Task<List<Harddrive>> GetHarddrivesParameters()
    {
        return Task.Run(() => (List<Harddrive>)_cache.Get("harddrive"));
    }

    public Task<Motherboard> GetMotherboardParameters()
    {
        return Task.Run(() => (Motherboard)_cache.Get("motherboard"));
    }

    public Task<List<Ram>> GetRamsParameters()
    {
        return Task.Run(() => (List<Ram>)_cache.Get("ram"));
    }

    public Task<SystemInfo> GetSystemInfo()
    {
        return Task.Run(() => (SystemInfo)_cache.Get("systemInfo"));
    }
}
