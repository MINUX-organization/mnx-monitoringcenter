namespace MINUX.Backend.Unit.UseCases.Abstractions;

public interface IGenericRepository<T> where T : class
{
    Task Remove(Guid id);
}
