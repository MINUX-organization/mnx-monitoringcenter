using MediatR;
using Microsoft.AspNetCore.Mvc;
using MINUX.Backend.Unit.Core;
using MINUX.Backend.Unit.UseCases.Queries.GetMinersQuery;

namespace MINUX.Backend.Unit.Service.Controllers;

/// <summary>
/// Предоставлет API для работы с майнерами
/// </summary>
[Route("api/miner")]
[ApiController]
public class MinerController : ControllerBase
{
    private readonly IMediator _mediator;

    public MinerController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// Получить список доступных майнеров
    /// </summary>
    /// <returns> Список доступных майнеров </returns>
    [HttpGet("available")]
    public IAsyncEnumerable<Miner> GetAvailable()
    {
        return _mediator.CreateStream(new GetAvailableMinersQuery());
    }
}
