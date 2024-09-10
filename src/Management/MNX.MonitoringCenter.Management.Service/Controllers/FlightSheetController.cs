using MediatR;
using Microsoft.AspNetCore.Mvc;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Infrastructure;
using MNX.MonitoringCenter.Management.Contracts.FlightSheet;
using MNX.MonitoringCenter.Management.UseCases.FlightSheet.Queries;
using MNX.MonitoringCenter.Management.UseCases.FlightSheets.Commands.CreateFlightSheet;
using MNX.MonitoringCenter.Management.UseCases.FlightSheets.Commands.EditFightSheet;
using MNX.MonitoringCenter.Management.UseCases.FlightSheets.Commands.Models;
using MNX.MonitoringCenter.Management.UseCases.FlightSheets.Commands.RemoveFlightSheet;

namespace MNX.MonitoringCenter.Management.Service.Controllers.Configurations;

/// <summary>
/// Предоставляет API для работы с полётными листами.
/// </summary>
[Route("api/flight_sheets")]
[ApiController]
public class FlightSheetController : ControllerBase
{
    private readonly IMediator _mediator;

    private readonly UserAccessor _userAccessor;

    public FlightSheetController(IMediator mediator, UserAccessor userAccessor)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _userAccessor = userAccessor ?? throw new ArgumentNullException(nameof(userAccessor));
    }

    /// <summary>
    /// Получить список всех полётных листов.
    /// </summary>
    /// <returns> Список полётных листов. </returns>
    /// <response code="200"> Успешно. </response>
    [HttpGet]
    [ProducesResponseType(typeof(IAsyncEnumerable<FlightSheetModelBase>), 200)]
    public IAsyncEnumerable<FlightSheetModelBase> GetList()
    {
        var userId = _userAccessor.GetUserId();
        return _mediator.CreateStream(new GetFlightSheetsQuery(userId));
    }

    /// <summary>
    /// Создать полётный лист.
    /// </summary>
    /// <param name="model"> Модель полётного листа. </param>
    /// <returns> Результат создания полётного листа. </returns>
    /// <response code="201"> Успешно. </response>
    /// <response code="400"> Введены некорректные данные. </response>
    [HttpPost]
    [ProducesResponseType(typeof(IEnumerable<FlightSheetModelBase>), 201)]
    [ProducesResponseType(typeof(IEnumerable<string>), 400)]
    public async Task<IActionResult> Create(FlightSheetInputModel model)
    {
        var userId = _userAccessor.GetUserId();
        var result = await _mediator.Send(new CreateFlightSheetCommand(userId, model));
        return result.ToActionResult();
    }

    /// <summary>
    /// Обновить полётный лист.
    /// </summary>
    /// <param name="id"> Идентификатор полётного листа. </param>
    /// <param name="model"> Модель полётного листа. </param>
    /// <returns> Результат обновления полётного листа. </returns>
    /// <response code="204"> Успешно. </response>
    /// <response code="400"> Введены некорректные данные. </response>
    [HttpPut("{id:Guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(IEnumerable<string>), 400)]
    public async Task<IActionResult> Edit(Guid id, FlightSheetInputModel model)
    {
        var userId = _userAccessor.GetUserId();
        var result = await _mediator.Send(new EditFlightSheetCommand(id, userId, model));
        return result.ToActionResult();
    }

    /// <summary>
    /// Удалить полётный лист.
    /// </summary>
    /// <param name="id"> Идентификатор полётного листа. </param>
    /// <returns> Результат удаления полётного листа. </returns>
    /// <response code="204"> Успешно. </response>
    [HttpDelete("{id:Guid}")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> Remove(Guid id)
    {
        var userId = _userAccessor.GetUserId();
        var result = await _mediator.Send(new RemoveFlightSheetCommand(id, userId));
        return result.ToActionResult();
    }
}
