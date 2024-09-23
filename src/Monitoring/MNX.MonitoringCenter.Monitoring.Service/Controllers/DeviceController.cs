using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Infrastructure;
using MNX.MonitoringCenter.Inventory.Contracts.Gpu.Restrictions;
using MNX.MonitoringCenter.Inventory.UseCases.Gpu;

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
    /// Получить уникальный названия зарегистрированных видеокарт.
    /// </summary>
    /// <returns> Уникальный названия видеокарт. </returns>
    /// <response code="200"> Успешно </response>
    [HttpGet("gpus/unique_names")]
    [ProducesResponseType(typeof(IAsyncEnumerable<string>), 200)]
    public IAsyncEnumerable<string> GetGpuUniqueNames()
    {
        var userId = _userAccessor.GetUserId();
        return _mediator.CreateStream(new GetGpuUniqueNamesQuery(userId));
    }

    /// <summary>
    /// Получить ограничения видеокарты по её полному названию.
    /// </summary>
    /// <param name="gpuName"> Название видеокарты. </param>
    /// <returns> Ограничения. </returns>
    /// <response code="200"> Успешно </response>
    [HttpGet("gpus/{gpuName}/restrictions")]
    [ProducesResponseType(typeof(GpuRestrictions), 200)]
    public async Task<IActionResult> GetGpuRestrictions(string gpuName)
    {
        var result = await _mediator.Send(new GetGpuRestrictionsQuery(gpuName));
        return result.ToActionResult();
    }

    /*/// <summary>
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
        var result = await _mediator.Send(new GetGpuOverclockingQuery(id));
        return result.ToActionResult();
    }*/
}
