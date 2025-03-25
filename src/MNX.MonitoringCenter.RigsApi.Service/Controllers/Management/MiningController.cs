using MediatR;
using Microsoft.AspNetCore.Mvc;
using MNX.Application.UseCases.Results;
using Microsoft.AspNetCore.Authorization;
using MNX.MonitoringCenter.Management.UseCases.Mining;
using MNX.MonitoringCenter.RigsApi.Service.Infrastructure;

namespace MNX.MonitoringCenter.RigsApi.Service.Controllers.Management;

/// <summary>
/// Майнинг контроллер.
/// </summary>
[Authorize]
[ApiController]
[Route("api/rigs")]
public class MiningController : ControllerBase
{
    private readonly Guid _userId;

    private readonly IMediator _mediator;

    public MiningController(IMediator mediator, UserAccessor userAccessor)
    {
        _userId = userAccessor.GetUserId();
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// Запустить майнинг.
    /// </summary>
    /// <param name="rigId"> Идентификатор рига. </param>
    /// <response code="204"> Успешно. </response>
    /// <response code="400"> Риг не найден. </response>
    /// <returns> Результат запуска майнинга. </returns>
    [HttpPost("{rigId:Guid}/mining/start")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> Start(Guid rigId)
    {
        var result = await _mediator.Send(new StartMiningCommand(rigId, _userId));
        return result.ToActionResult();
    }

    /// <summary>
    /// Остановить майнинг.
    /// </summary>
    /// <param name="rigId"> Идентификатор рига. </param>
    /// <response code="204"> Успешно. </response>
    /// <response code="400"> Риг не найден. </response>
    /// <returns> Результат остановки майнинга. </returns>
    [HttpPost("{rigId:Guid}/mining/stop")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> Stop(Guid rigId)
    {
        var result = await _mediator.Send(new StopMiningCommand(rigId, _userId));
        return result.ToActionResult();
    }
}
