namespace MNX.MonitoringCenter.Management.UseCases.Abstractions;

public interface IAlgorithmRepository
{
    IAsyncEnumerable<string> GetNamesOfAvailableAlgorithms();

    Task<bool> Exists(string name);
}
