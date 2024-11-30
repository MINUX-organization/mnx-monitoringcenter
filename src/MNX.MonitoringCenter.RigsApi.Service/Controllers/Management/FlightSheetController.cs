using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Management.Contracts.FlightSheet;
using MNX.MonitoringCenter.Management.UseCases.FlightSheet.Commands.ApplyFlightSheet;
using MNX.MonitoringCenter.Management.UseCases.FlightSheet.Commands.CreateFlightSheet;
using MNX.MonitoringCenter.Management.UseCases.FlightSheet.Commands.EditFightSheet;
using MNX.MonitoringCenter.Management.UseCases.FlightSheet.Commands.Models;
using MNX.MonitoringCenter.Management.UseCases.FlightSheet.Commands.RemoveFlightSheet;
using MNX.MonitoringCenter.Management.UseCases.FlightSheet.Queries;
using MNX.MonitoringCenter.RigsApi.Service.Infrastructure;
using MNX.MonitoringCenter.RigsApi.UseCases.Devices.Queries.GetMiningDevices;
using MNX.MonitoringCenter.RigsApi.UseCases.Devices.Queries.GetMiningDevices.GetFlightSheetDevices;

namespace MNX.MonitoringCenter.RigsApi.Service.Controllers.Management;

/// <summary>
/// Предоставляет API для работы с полётными листами.
/// </summary>
[Route("api/flight_sheets")]
[ApiController]
[Authorize]
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
    [ProducesResponseType(typeof(IAsyncEnumerable<FlightSheetModel>), 200)]
    public IAsyncEnumerable<FlightSheetModel> GetList()
    {
        var userId = _userAccessor.GetUserId();
        return _mediator.CreateStream(new GetFlightSheetsQuery(userId));
    }

    /// <summary>
    /// Получить список майнинг устройств, к котором применён полётный лист.
    /// </summary>
    /// <param name="id"> Идентификатор полётного листа. </param>
    /// <returns> Список майнинг устройств, сгруппированных по ригу и по типу. </returns>
    [HttpGet("{id:Guid}/devices")]
    [ProducesResponseType(typeof(IAsyncEnumerable<Group<Group<MiningDevice>>>), 200)]
    public IAsyncEnumerable<Group<Group<MiningDevice>>> GetMiningDevices(Guid id)
    {
        var userId = _userAccessor.GetUserId();
        return _mediator.CreateStream(new GetFlightSheetMiningDevicesQuery(userId, id));
    }

    /// <summary>
    /// Получить список поддерживаемых майнинг устройств.
    /// </summary>
    /// <param name="id"> Идентификатор полётного листа. </param>
    /// <returns> Список поддерживаемых майнинг устройств, сгруппированных по ригу и по типу. </returns>
    [HttpGet("{id:Guid}/devices/supported")]
    [ProducesResponseType(typeof(IAsyncEnumerable<Group<Group<MiningDevice>>>), 200)]
    public IAsyncEnumerable<Group<Group<MiningDevice>>> GetSupportedMiningDevices(Guid id)
    {
        var userId = _userAccessor.GetUserId();
        return _mediator.CreateStream(new GetFlightSheetSupportedMiningDevicesQuery(userId, id));
    }

    /// <summary>
    /// Получить полётный лист по идентификатору.
    /// </summary>
    /// <param name="id"> Идентификатор. </param>
    /// <returns> Полётный лист. </returns>
    /// <response code="200"> Успешно. </response>
    /// <response code="400"> Полётный лист не найден. </response>
    [HttpGet("{id:Guid}")]
    [ProducesResponseType(typeof(FlightSheetModel), 200)]
    [ProducesResponseType(typeof(IEnumerable<string>), 400)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var userId = _userAccessor.GetUserId();
        var result = await _mediator.Send(new GetFlightSheetByIdQuery(id, userId));
        return result.ToActionResult();
    }

    /// <summary>
    /// Применить полётный лист.
    /// </summary>
    /// <param name="id"> Идентификатор полётного листа. </param>
    /// <param name="miningDevicesIds"> Идентификаторы майнинг устройств. </param>
    /// <returns> Идентификаторы устройств, на которые применился полётный лист. </returns>
    [HttpPost("{id:Guid}/apply")]
    [ProducesResponseType(typeof(IEnumerable<Guid>), 201)]
    [ProducesResponseType(typeof(IEnumerable<string>), 400)]
    public async Task<IActionResult> Apply(Guid id, Guid[] miningDevicesIds)
    {
        var userId = _userAccessor.GetUserId();
        var result = await _mediator.Send(new ApplyFlightSheetCommand(userId, id, miningDevicesIds));
        return result.ToActionResult();
    }

    /// <summary>
    /// Создать полётный лист.
    /// </summary>
    /// <param name="model"> Модель полётного листа. </param>
    /// <returns> Результат создания полётного листа. </returns>
    /// <response code="201"> Успешно. </response>
    /// <response code="400"> Введены некорректные данные. </response>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), 201)]
    [ProducesResponseType(typeof(IEnumerable<string>), 400)]
    public async Task<IActionResult> Create(FlightSheetInputModel model)
    {
        var userId = _userAccessor.GetUserId();
        var result = await _mediator.Send(new CreateFlightSheetCommand(userId, model));
        return result.ToActionResult();
    }

    /// <summary>
    /// Редактировать полётный лист.
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
