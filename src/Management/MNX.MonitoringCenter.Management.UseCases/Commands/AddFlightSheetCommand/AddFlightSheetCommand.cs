using Kernel.UseCases;
using MediatR;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.CreateCryptocurrencyCommand;

/// <summary>
/// Команда добавления полётного листа
/// </summary>
public class AddFlightSheetCommand : IRequest<Result<Unit>>
{
    /// <summary>
    /// Название полётного листа
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Майнер
    /// </summary>
    public string Miner { get; }

    /// <summary>
    /// Криптовалюта
    /// </summary>
    public string Cryprocurrency { get; }

    /// <summary>
    /// Адрес кашелька
    /// </summary>
    public string WalletAddress { get; }

    /// <summary>
    /// Адрес пула в формате: host:port
    /// </summary>
    public string PoolAddress { get; }
}