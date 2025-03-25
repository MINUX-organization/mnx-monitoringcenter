using MediatR;
using Microsoft.AspNetCore.Mvc;
using MNX.Application.UseCases.Results;
using Microsoft.AspNetCore.Authorization;
using MNX.MonitoringCenter.Management.Contracts.Miner;
using MNX.MonitoringCenter.RigsApi.Service.Infrastructure;
using MNX.MonitoringCenter.Management.UseCases.Mining.Miner.Queries;
using MNX.MonitoringCenter.Management.UseCases.Mining.Miner.Commands.Models;
using MNX.MonitoringCenter.Management.UseCases.Mining.Miner.Commands.EditMinerCommand;
using MNX.MonitoringCenter.Management.UseCases.Mining.Miner.Commands.DeleteMinerCommand;
using MNX.MonitoringCenter.Management.UseCases.Mining.Miner.Commands.CreateCustomMinerCommand;

namespace MNX.MonitoringCenter.RigsApi.Service.Controllers.Management;

/// <summary>
/// Предоставляет API для работы с майнерами
/// </summary>
[Route("api/miners")]
[ApiController]
[Authorize]
public class MinerController : ControllerBase
{
    /// <summary>
    /// Медиатор.
    /// </summary>
    private readonly IMediator _mediator;

    /// <summary>
    /// Сервис для доступа к данным пользователя.
    /// </summary>
    private readonly UserAccessor _accessor;

    public MinerController(IMediator mediator, UserAccessor accessor)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _accessor = accessor ?? throw new ArgumentNullException(nameof(accessor));
    }

    /// <summary>
    /// Получить список доступных майнеров.
    /// </summary>
    /// <returns> Список доступных майнеров. </returns>
    /// <response code="200"> Успешно. </response>
    [HttpGet("available")]
    [ProducesResponseType(typeof(IAsyncEnumerable<MinerModel>), 200)]
    public IAsyncEnumerable<MinerModel> GetAvailable()
    {
        return _mediator.CreateStream(new GetAvailableMinersQuery(_accessor.GetUserId()));
    }

    /// <summary>
    /// Добавить кастомный майнер.
    /// </summary>
    /// <param name="request"> Модель запроса. </param>
    /// <response code="201"> Успешно создано. </response>
    /// <response code="409"> Майнер с таким названием уже существует. </response>
    [HttpPost("custom")]
    [ProducesResponseType(typeof(MinerModel), 201)]
    [ProducesResponseType(typeof(List<string>), 409)]
    public async Task<IActionResult> AddCustom(MinerInputModel request)
    {
        var result = await _mediator.Send(new CreateCustomMinerCommand(request, _accessor.GetUserId()));
        return result.ToActionResult();
    }

    /// <summary>
    /// Обновить кастомный минер.
    /// </summary>
    /// <param name="model"> Модель запроса. </param>
    /// <param name="minerId"> Идентификатор майнера. </param>
    /// <response code="204"> Успешно. </response>
    /// <response code="400"> Майнера с заданным идентификатором не существует. </response>
    /// <response code="409"> Майнер с данным названием уже существует. </response>
    [HttpPut("custom/{minerId:Guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(List<string>), 400)]
    [ProducesResponseType(typeof(List<string>), 409)]
    public async Task<IActionResult> UpdateCustom(MinerInputModel model, Guid minerId)
    {
        var result = await _mediator.Send(new EditMinerCommand(model, minerId, _accessor.GetUserId()));
        return result.ToActionResult();
    }

    /// <summary>
    /// Удалить кастомный майнер.
    /// </summary>
    /// <param name="minerId"> Идентификатор майнера. </param>
    /// <response code="204"> Успешно. </response>
    [HttpDelete("custom/{minerId:Guid}")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> DeleteCustom(Guid minerId)
    {
        var result = await _mediator.Send(new DeleteMinerCommand(minerId, _accessor.GetUserId()));
        return result.ToActionResult();
    }
}