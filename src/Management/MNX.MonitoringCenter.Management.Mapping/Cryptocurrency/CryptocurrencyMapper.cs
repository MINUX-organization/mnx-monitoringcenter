using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Management.Core.Mining;
using MNX.MonitoringCenter.Management.UseCases.Mining.Cryptocurrency;
using MNX.MonitoringCenter.Management.UseCases.Mining.Cryptocurrency.Commands;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Cryptocurrency;

using Cryptocurrency = Core.Mining.Cryptocurrency;

/// <summary>
/// Реализация <see cref="ICryptocurrencyMapper"/>.
/// </summary>
public class CryptocurrencyMapper : ICryptocurrencyMapper
{
    /// <inheritdoc/>
    public Cryptocurrency MapToCoreEntity(CryptocurrencyInputModel model, Guid userId)
    {
        return new Cryptocurrency()
        {
            FullName = model.FullName,
            ShortName = model.ShortName,
            AlgorithmId = model.AlgorithmId,
            OwnerId = userId
        };
    }

    /// <inheritdoc/>
    public CryptocurrencyModel MapToModel(Cryptocurrency entity)
    {
        var algorithm = new Algorithm()
        {
            Id = entity.AlgorithmId,
            Name = entity.Algorithm!.Name,
            OwnerId = entity.Algorithm.OwnerId,
        };

        return new CryptocurrencyModel()
        {
            Id = entity.Id,
            Algorithm = algorithm,
            FullName = entity.FullName,
            ShortName = entity.ShortName,
            OwnerId = entity.OwnerId
        };
    }
}
