using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MNX.MonitoringCenter.Management.Core.Miner;
using MNX.MonitoringCenter.Management.UseCases.Miner.Queries;
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
    /// Сервис для доступ к данным пользователя.
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
    [ProducesResponseType(typeof(IAsyncEnumerable<Miner>), 200)]
    public IAsyncEnumerable<Miner> GetAvailable()
    {
        return _mediator.CreateStream(new GetAvailableMinersQuery(_accessor.GetUserId()));
    }
}
