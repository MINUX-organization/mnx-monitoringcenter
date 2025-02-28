using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Management.Contracts.Miner;
using MNX.MonitoringCenter.Management.UseCases.Mining.Miner.Commands.CreateCustomMinerCommand;
using MNX.MonitoringCenter.Management.UseCases.Mining.Miner.Commands.DeleteMinerCommand;
using MNX.MonitoringCenter.Management.UseCases.Mining.Miner.Commands.EditMinerCommand;
using MNX.MonitoringCenter.Management.UseCases.Mining.Miner.Commands.Models;
using MNX.MonitoringCenter.Management.UseCases.Mining.Miner.Queries;
using MNX.MonitoringCenter.RigsApi.Service.Infrastructure;

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
    /// Получить список доступных майнеров
    /// </summary>
    /// <returns> Список доступных майнеров </returns>
    /// <response code="200"> Успешно </response>
    [HttpGet("available")]
    [ProducesResponseType(typeof(IAsyncEnumerable<MinerModel>), 200)]
    public IAsyncEnumerable<MinerModel> GetAvailable()
    {
        return _mediator.CreateStream(new GetAvailableMinersQuery(_accessor.GetUserId()));
    }

    /// <summary>
    /// Добавить кастомный майнер.
    /// </summary>
    /// <param name="request"> Модель запроса </param>
    /// <response code="201"> Успешно создано </response>
    [HttpPost("custom")]
    [ProducesResponseType(typeof(MinerModel), 201)]
    public async Task<IActionResult> AddCustom(MinerInputModel request)
    {
        var result = await _mediator.Send(new CreateCustomMinerCommand(request, _accessor.GetUserId()));
        return result.ToActionResult();
    }

    /// <summary>
    /// Обновить кастомный минер.
    /// </summary>
    /// <param name="request">Модель запроса</param>
    /// <param name="minerId">Идентификатор майнера</param>
    /// <response code="204"> Успешно </response>
    [HttpPut("custom/{minerId:Guid}")]
    public async Task<IActionResult> UpdateCustom(MinerInputModel request, Guid minerId)
    {
        var result = await _mediator.Send(new EditMinerCommand(request, minerId, _accessor.GetUserId()));
        return result.ToActionResult();
    }

    /// <summary>
    /// Удалить кастомный майнер
    /// </summary>
    /// <param name="minerId">Идентификатор майнера</param>
    /// <response code="204"> Успешно </response>
    [HttpDelete("custom/{minerId:Guid}")]
    public async Task<IActionResult> DeleteCustom(Guid minerId)
    {
        var result = await _mediator.Send(new DeleteMinerCommand(minerId, _accessor.GetUserId()));
        return result.ToActionResult();
    }
}