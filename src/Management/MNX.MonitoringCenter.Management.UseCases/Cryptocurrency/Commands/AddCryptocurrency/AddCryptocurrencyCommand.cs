using MNX.Application.UseCases;
using MNX.MonitoringCenter.Management.Contracts;

namespace MNX.MonitoringCenter.Management.UseCases.Cryptocurrency.Commands.AddCryptocurrency;

/// <summary>
/// Команда добавления криптовалюты.
/// </summary>
public class AddCryptocurrencyCommand : IValidatableCommand<CryptocurrencyModel>
{
    /// <summary>
    /// Входная модель крипты.
    /// </summary>
    public CryptocurrencyInputModel Model { get; }

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; }

    public AddCryptocurrencyCommand(CryptocurrencyInputModel model, Guid userId)
    {
        Model = model;
        UserId = userId;
    }
}