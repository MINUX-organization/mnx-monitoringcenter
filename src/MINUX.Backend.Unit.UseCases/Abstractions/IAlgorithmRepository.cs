using MINUX.Backend.Unit.Core;

namespace MINUX.Backend.Unit.UseCases.Abstractions;

public interface IAlgorithmRepository
{
    IAsyncEnumerable<Algorithm> GetAll();

    Task<bool> Exists(string name);
}
