namespace MNX.MonitoringCenter.Inventory.UseCases.Cpu;

using Cpu = Contracts.Cpu.Cpu;

public interface ICpuRepository
{
    /// <summary>
    /// Получить список процессоров.
    /// </summary>
    /// <returns> Список процессоров. </returns>
    IAsyncEnumerable<Cpu> GetList();
}
