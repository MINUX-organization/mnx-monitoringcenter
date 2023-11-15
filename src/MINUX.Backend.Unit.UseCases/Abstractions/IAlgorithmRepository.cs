namespace MINUX.Backend.Unit.UseCases.Abstractions;

public interface IAlgorithmRepository
{
    IAsyncEnumerable<string> GetNamesOfAvailableAlgorithms();

    Task<bool> Exists(string name);
}
