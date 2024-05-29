using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MNX.MonitoringCenter.Infrastructure;
using MNX.MonitoringCenter.Monitoring.UseCases.Queries.Devices.GetCpusInfo;
using MNX.MonitoringCenter.Monitoring.UseCases.Queries.Devices.GetGpuOverclocking;
using MNX.MonitoringCenter.Monitoring.UseCases.Queries.Devices.GetGpusInfo;

namespace MNX.MonitoringCenter.Monitoring.Service.Controllers;

/// <summary>
/// Предоставляет API для работы с майнинг устройствами.
/// </summary>
[ApiController]
[Route("api/devices")]
[Authorize]
public class DeviceController : ControllerBase
{
    /// <summary>
    /// Посредник.
    /// </summary>
    private readonly IMediator _mediator;

    /// <summary>
    /// Сервис для доступа к данным пользователя.
    /// </summary>
    private readonly UserAccessor _userAccessor;

    public DeviceController(IMediator mediator, UserAccessor userAccessor)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _userAccessor = userAccessor ?? throw new ArgumentNullException(nameof(userAccessor));
    }

    /// <summary>
    /// Получить информацию о видеокартах.
    /// </summary>
    [HttpGet("gpus")]
    public IAsyncEnumerable<GpuInfo> GetGpusInfo()
    {
        var userId = _userAccessor.GetUserId();
        return _mediator.CreateStream(new GetGpusInfoQuery(userId));
    }

    /// <summary>
    /// Получить информацию о процессорах.
    /// </summary>
    [HttpGet("cpus")]
    public IAsyncEnumerable<CpuInfo> GetCpusInfo()
    {
        var userId = _userAccessor.GetUserId();
        return _mediator.CreateStream(new GetCpusInfoQuery(userId));
    }

    /// <summary>
    /// Получить разгон видеокарты.
    /// </summary>
    [HttpGet("gpus/{id:Guid}/overclocking")]
    public async Task<IActionResult> GetGpuOverclocking(Guid id)
    {
        var result = await _mediator.Send(new GetOverclockingQuery(id));

        if (result.IsSuccess)
            return Ok(result.GetValue());
        else
            return BadRequest();
    }
}
