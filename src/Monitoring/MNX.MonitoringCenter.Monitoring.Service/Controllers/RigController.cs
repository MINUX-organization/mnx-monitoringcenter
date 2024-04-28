using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Infrastructure;
using MNX.MonitoringCenter.Monitoring.Core;
using MNX.MonitoringCenter.Monitoring.UseCases.Queries;
using MNX.MonitoringCenter.Monitoring.UseCases.Queries.GetRigsInformation;
using MNX.MonitoringCenter.Monitoring.UseCases.Queries.GetSummarizedQuantitativeData;

namespace MNX.MonitoringCenter.Monitoring.Service.Controllers;

/// <summary>
/// Предоставляет API для работы с ригами.
/// </summary>
[ApiController]
[Route("api/rigs")]
[Authorize]
public class RigController : ControllerBase
{
    /// <summary>
    /// Медиатор.
    /// </summary>
    private readonly IMediator _mediator;

    /// <summary>
    /// Сервис для доступа к данным пользователя.
    /// </summary>
    private readonly UserAccessor _userAccessor;

    public RigController(IMediator mediator, UserAccessor userAccessor)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _userAccessor = userAccessor ?? throw new ArgumentNullException(nameof(userAccessor));
    }

    /// <summary>
    /// Получить информацию о ригах.
    /// </summary>
    /// <returns> Информация о ригах. </returns>
    [HttpGet("info")]
    [ProducesResponseType(typeof(IAsyncEnumerable<RigInformationMessage>), 200)]
    public IAsyncEnumerable<RigInformationMessage> GetRigsInfo()
    {
        var userId = _userAccessor.GetUserId();
        return _mediator.CreateStream(new GetRigsInformationQuery(new Specification(userId)));
    }

    /// <summary>
    /// Получить обобщённые количественные данные.
    /// </summary>
    /// <returns> Результат получения обобщённых количественных данных. </returns>
    [HttpGet("totalData")]
    [ProducesResponseType(typeof(RigsSummarizedQuantitativeData), 200)]
    public async Task<IActionResult> GetSummarizedQuantitativeData()
    {
        var userId = _userAccessor.GetUserId();
        var result = await _mediator.Send(new GetRigsSummarizedQuantitativeDataQuery(new Specification(userId)));
        return result.ToActionResult();
    }
}
