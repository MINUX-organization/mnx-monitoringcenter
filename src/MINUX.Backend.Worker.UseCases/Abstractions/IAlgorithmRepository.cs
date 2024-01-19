namespace MINUX.Backend.Worker.UseCases.Abstractions;

public interface IAlgorithmRepository
{
    IAsyncEnumerable<string> GetNamesOfAvailableAlgorithms();

    Task<bool> Exists(string name);
}
