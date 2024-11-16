using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Algorithm.Queries;
using MNX.MonitoringCenter.RigsApi.Service.Infrastructure;

namespace MNX.MonitoringCenter.RigsApi.Service.Controllers.Management;

/// <summary>
/// Содержит эндпоинты, связанные с алгоритмами
/// </summary>
[Route("api/algorithms")]
[ApiController]
[Authorize]
public class AlgorithmController : ControllerBase
{
    /// <summary>
    /// Медиатор.
    /// </summary>
    private readonly IMediator _mediator;

    /// <summary>
    /// Сервис для доступ к данным пользователя.
    /// </summary>
    private readonly UserAccessor _accessor;

    public AlgorithmController(IMediator mediator, UserAccessor accessor)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _accessor = accessor ?? throw new ArgumentNullException(nameof(accessor));
    }

    /// <summary>
    /// Получить список доступных алгоритмов
    /// </summary>
    /// <returns> Список доступных алгоритмов </returns>
    /// <response code="200"> Успешно </response>
    [HttpGet("available")]
    [ProducesResponseType(typeof(IAsyncEnumerable<Algorithm>), 200)]
    public IAsyncEnumerable<Algorithm> GetAvailable()
    {
        return _mediator.CreateStream(new GetAvailableAlgorithmsQuery(_accessor.GetUserId()));
    }
}