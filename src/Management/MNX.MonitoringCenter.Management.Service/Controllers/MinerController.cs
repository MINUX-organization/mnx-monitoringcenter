using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MNX.MonitoringCenter.Management.UseCases.Miner.Models;
using MNX.MonitoringCenter.Management.UseCases.Miner.Queries;

namespace MNX.MonitoringCenter.Management.Service.Controllers;

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

    public MinerController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// Получить список доступных майнеров
    /// </summary>
    /// <returns> Список доступных майнеров </returns>
    /// <response code="200"> Успешно </response>
    [HttpGet("available")]
    [ProducesResponseType(typeof(IAsyncEnumerable<MinerOutputModel>), 200)]
    public IAsyncEnumerable<MinerOutputModel> GetAvailable()
    {
        return _mediator.CreateStream(new GetAvailableMinersQuery());
    }
}
