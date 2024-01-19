using MediatR;
using Microsoft.AspNetCore.Mvc;
using MINUX.Backend.Worker.UseCases.Queries.GetAlgorithmsQuery;

namespace MINUX.Backend.Worker.Service.Controllers;

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
    [HttpGet("available")]
    public IAsyncEnumerable<string> GetAvailable()
    {
        return _mediator.CreateStream(new GetAvailableAlgorithmsQuery());
    }
}