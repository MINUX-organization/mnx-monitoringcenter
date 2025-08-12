using MNX.MonitoringCenter.Management.Contracts.FlightSheet;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Commands;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Commands.EditFightSheet;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Commands.Models;
using MNX.MonitoringCenter.Management.UseCases.Mining.Miner;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.FlightSheet;

using FlightSheet = Core.Mining.FlightSheet.FlightSheet;

/// <summary>
/// Реализация <see cref="IFlightSheetMapper"/>.
/// </summary>
public partial class FlightSheetMapper : IFlightSheetMapper
{
    private readonly IMinerMapper _minerMapper;

    private readonly IMiningConfigMapper _miningConfigMapper;

    ///
    public FlightSheetMapper(IMinerMapper minerMapper,
                             IMiningConfigMapper miningConfigMapper)
    {
        _minerMapper = minerMapper ?? throw new ArgumentNullException(nameof(minerMapper));
        _miningConfigMapper = miningConfigMapper ?? throw new ArgumentNullException(nameof(miningConfigMapper));
    }

    /// <inheritdoc/>
    public FlightSheet MapToCoreEntity(FlightSheetInputModel model, Guid userId)
    {
        var flightSheetId = Guid.NewGuid();
        var targets = MapInputTargetsToCore(model.Targets, flightSheetId);
        return new FlightSheet()
        {
            Id = flightSheetId,
            Name = model.Name,
            OwnerId = userId,
            Targets = targets
        };
    }

    /// <inheritdoc/>
    public FlightSheet MapToCoreEntity(EditFlightSheetCommand model)
    {
        var targets = MapInputTargetsToCore(model.Model.Targets, model.Id);
        return new FlightSheet()
        {
            Id = model.Id,
            Name = model.Model.Name,
            OwnerId = model.UserId,
            Targets = targets
        };
    }

    /// <inheritdoc/>
    public FlightSheetModel MapToModel(FlightSheet entity)
    {
        var targets = MapTargetsToModel(entity.Targets);
        return new FlightSheetModel()
        {
            Id = entity.Id,
            Name = entity.Name,
            Targets = targets
        };
    }
}
