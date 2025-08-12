using MNX.MonitoringCenter.Management.Contracts.FlightSheet;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Commands.EditFightSheet;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Commands.Models;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet;

using FlightSheet = Core.Mining.FlightSheet.FlightSheet;

/// <summary>
/// Интерфейс маппера сущности <see cref="FlightSheet"/> и её моделей.
/// </summary>
public interface IFlightSheetMapper
{
    /// <summary>
    /// Преобразовать сущность <see cref="FlightSheetInputModel"/> в <see cref="FlightSheet"/>
    /// с использованием идентификатора пользователя.
    /// </summary>
    /// <param name="model"> Модель данных. </param>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <returns> Новый экземпляр <see cref="FlightSheet"/>. </returns>
    FlightSheet MapToCoreEntity(FlightSheetInputModel model, Guid userId);

    /// <summary>
    /// Преобразовать сущность <see cref="EditFlightSheetCommand"/> в <see cref="FlightSheet"/>.
    /// </summary>
    /// <param name="model"> Модель данных. </param>
    /// <returns> Новый экземпляр <see cref="FlightSheet"/>. </returns>
    FlightSheet MapToCoreEntity(EditFlightSheetCommand model);

    /// <summary>
    /// Преобразовать сущность <see cref="FlightSheet"/> в <see cref="FlightSheetModel"/>.
    /// </summary>
    /// <param name="entity"> Модель данных. </param>
    /// <returns> Новый экземпляр <see cref="FlightSheetModel"/>. </returns>
    FlightSheetModel MapToModel(FlightSheet entity);
}
