using MNX.Application.UseCases;
using MNX.MonitoringCenter.Management.Core;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Crypto.AddCryptocurrency;

/// <summary>
/// Команда добавления криптовалюты
/// </summary>
public class AddCryptocurrencyCommand : IValidatableCommand<Cryptocurrency>
{
    /// <summary>
    /// Входная модель крипты.
    /// </summary>
    public CryptocurrencyInputModel Model { get; }

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public long UserId { get; }

    public AddCryptocurrencyCommand(CryptocurrencyInputModel model, long userId)
    {
        Model = model;
        UserId = userId;
    }
}