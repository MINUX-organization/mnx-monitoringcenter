using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Restrictions;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Cpu.GetCpusDetails;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Gpu;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Gpu.GetGpusDetails;
using MNX.MonitoringCenter.RigsApi.Service.Infrastructure;

namespace MNX.MonitoringCenter.RigsApi.Service.Controllers;

/// <summary>
/// Контроллер, предоставляющий rest api для устройств.
/// </summary>
[Authorize]
[ApiController]
[Route("api/devices")]
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
    /// Получить список видеокарт.
    /// </summary>
    /// <returns> Асинхронный поток видеокарт. </returns>
    [HttpGet("gpus")]
    [ProducesResponseType(typeof(IAsyncEnumerable<GpuDetails>), 200)]
    public IAsyncEnumerable<GpuDetails> GetGpus()
    {
        var userId = _userAccessor.GetUserId();
        return _mediator.CreateStream(new GetGpusDetailsQuery(userId));
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

    /// <summary>
    /// Получить список процессоров.
    /// </summary>
    /// <returns> Асинхронный поток процессоров. </returns>
    [HttpGet("сpus")]
    [ProducesResponseType(typeof(IAsyncEnumerable<CpuDetails>), 200)]
    public IAsyncEnumerable<CpuDetails> GetСpus()
    {
        var userId = _userAccessor.GetUserId();
        return _mediator.CreateStream(new GetCpusDetailsQuery(userId));
    }
}
