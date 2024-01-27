using MINUX.Backend.Worker.Core.HardwareParameters;
using MINUX.Backend.Worker.Core.HardwareParameters.Cpu;
using MINUX.Backend.Worker.Core.HardwareParameters.Gpu;
using MINUX.Backend.Worker.Core.HardwareParameters.Harddrive;
using MINUX.Backend.Worker.Core.HardwareParameters.Motherboard;
using MINUX.Backend.Worker.UseCases.Abstractions;

namespace MINUX.Backend.Worker.DataAccess.Repositories;

public class HardwareParametersRepository : IHardwareParametersRepository
{
    public Task<List<Cpu>> GetCpusParameters()
    {
        throw new NotImplementedException();
    }

    public Task<List<Gpu>> GetGpusParameters()
    {
        throw new NotImplementedException();
    }

    public Task<List<Harddrive>> GetHarddrivesParameters()
    {
        throw new NotImplementedException();
    }

    public Task<Motherboard> GetMotherboardParameters()
    {
        throw new NotImplementedException();
    }

    public Task<Ram> GetRamParameters()
    {
        throw new NotImplementedException();
    }

    public Task<SystemInfo> GetSystemInfo()
    {
        throw new NotImplementedException();
    }
}
