using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Management.UseCases.Mining.Pool;
using MNX.MonitoringCenter.Management.UseCases.Mining.Pool.Commands.AddPool;
using MNX.MonitoringCenter.Management.UseCases.Mining.Pool.Commands.EditPool;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Pool;

using Pool = Core.Mining.Pool;

/// <summary>
/// Реализация <see cref="IPoolMapper"/>.
/// </summary>
public class PoolMapper : IPoolMapper
{
    /// <inheritdoc/>
    public Pool MapToCoreEntity(AddPoolCommand model)
    {
        return new Pool()
        {
            Port = model.Model.Port,
            Domain = model.Model.Domain,
            CryptocurrencyId = model.Model.CryptocurrencyId,
            Tls = model.Model.Tls,
            OwnerId = model.UserId
        };
    }

    /// <inheritdoc/>
    public Pool MapToCoreEntity(EditPoolCommand model)
    {
        return new Pool()
        {
            Id = model.Id,
            Port = model.Model.Port,
            Domain = model.Model.Domain,
            CryptocurrencyId = model.Model.CryptocurrencyId,
            Tls = model.Model.Tls,
            OwnerId = model.UserId
        };
    }

    /// <inheritdoc/>
    public PoolModel MapToModel(Pool entity)
    {
        return new PoolModel()
        {
            Id = entity.Id,
            Domain = entity.Domain,
            Port = entity.Port,
            Tls = entity.Tls,
            CryptocurrencyId = entity.Cryptocurrency!.Id,
            Cryptocurrency = entity.Cryptocurrency!.FullName,
            OwnerId = entity.OwnerId
        };
    }
}
