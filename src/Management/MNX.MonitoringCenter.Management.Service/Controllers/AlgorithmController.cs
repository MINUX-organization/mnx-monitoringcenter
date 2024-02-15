using MediatR;
using Microsoft.AspNetCore.Mvc;
using MNX.MonitoringCenter.Management.UseCases.Queries.GetAlgorithmsQuery;

namespace MNX.MonitoringCenter.Management.Service.Controllers;

/// <summary>
/// Содержит эндпоинты, связанные с алгоритмами
/// </summary>
[Route("api/algorithm")]
[ApiController]
public class AlgorithmController : ControllerBase
{
    private readonly IMediator _mediator;

    public AlgorithmController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// Получить список доступных алгоритмов
    /// </summary>
    /// <returns> Список доступных алгоритмов </returns>
    /// <response code="200"> Успешно </response>
    [HttpGet("available")]
    [ProducesResponseType(typeof(IAsyncEnumerable<string>), 200)]
    public IAsyncEnumerable<string> GetAvailable()
    {
        return _mediator.CreateStream(new GetAvailableAlgorithmsQuery());
    }
}