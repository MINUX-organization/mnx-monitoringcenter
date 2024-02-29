using Kernel.UseCases;
using MediatR;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Crypto.RemoveCryptocurrency;

/// <summary>
/// Команда удаления криптовалюты
/// </summary>
public class RemoveCryptocurrencyCommand : IRequest<Result<Unit>>
{
    /// <summary>
    /// Идентификатор криптовалюты
    /// </summary>
    public int Id { get; }

    public RemoveCryptocurrencyCommand(int id)
    {
        Id = id;
    }
}
