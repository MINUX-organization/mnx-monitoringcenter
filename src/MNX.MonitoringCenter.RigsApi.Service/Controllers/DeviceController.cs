using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Restrictions;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Gpu;
using MNX.MonitoringCenter.Management.Contracts.Overclocking;
using MNX.MonitoringCenter.Management.UseCases.Mining.MiningDevice.Commands.SetOverclocking;
using MNX.MonitoringCenter.Management.UseCases.Mining.MiningDevice.Queries;
using MNX.MonitoringCenter.RigsApi.Service.Infrastructure;
using MNX.MonitoringCenter.RigsApi.UseCases.Devices.Queries.GetCpus;
using MNX.MonitoringCenter.RigsApi.UseCases.Devices.Queries.GetGpus;
using MNX.SecurityManagement.Licensing.Integration;

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
    [ProducesResponseType(typeof(IAsyncEnumerable<GetGpusQueryResponse>), 200)]
    [WithoutLicenseChecking]
    public IAsyncEnumerable<GetGpusQueryResponse> GetGpus()
    {
        var userId = _userAccessor.GetUserId();
        return _mediator.CreateStream(new GetGpusQuery(userId));
    }

    /// <summary>
    /// Получить уникальные названия зарегистрированных видеокарт.
    /// </summary>
    /// <returns> Уникальный названия видеокарт. </returns>
    /// <response code="200"> Успешно. </response>
    [HttpGet("gpus/unique_names")]
    [ProducesResponseType(typeof(IAsyncEnumerable<string>), 200)]
    [WithoutLicenseChecking]
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
    /// <response code="200"> Успешно. </response>
    [HttpGet("gpus/{gpuName}/restrictions")]
    [ProducesResponseType(typeof(NvidiaGpuRestrictions), 200)]
    [WithoutLicenseChecking]
    public async Task<IActionResult> GetGpuRestrictions(string gpuName)
    {
        var result = await _mediator.Send(new GetGpuRestrictionsQuery(gpuName));
        return result.ToActionResult();
    }

    /// <summary>
    /// Получить ограничения видеокарты по её идентификатору.
    /// </summary>
    /// <param name="gpuId"> Идентификатор видеокарты. </param>
    /// <returns> Ограничения. </returns>
    /// <response code="200"> Успешно. </response>
    /// <response code="400"> Майнинг устройство не найдено. </response>
    [HttpGet("gpus/{gpuId:guid}/restrictions")]
    [ProducesResponseType(typeof(NvidiaGpuRestrictions), 200)]
    [ProducesResponseType(typeof(List<string>), 400)]
    [WithoutLicenseChecking]
    public async Task<IActionResult> GetGpuRestrictionsById(Guid gpuId)
    {
        var result = await _mediator.Send(new GetGpuRestrictionsByIdQuery(gpuId));
        return result.ToActionResult();
    }

    /// <summary>
    /// Получить список процессоров.
    /// </summary>
    /// <returns> Асинхронный поток процессоров. </returns>
    [HttpGet("cpus")]
    [ProducesResponseType(typeof(IAsyncEnumerable<GetCpusQueryResponse>), 200)]
    [WithoutLicenseChecking]
    public IAsyncEnumerable<GetCpusQueryResponse> GetСpus()
    {
        var userId = _userAccessor.GetUserId();
        return _mediator.CreateStream(new GetCpusQuery(userId));
    }

    /// <summary>
    /// Получить разгон майнинг устройства.
    /// </summary>
    /// <param name="deviceId"> Идентификатор майнинг устройства. </param>
    /// <returns> Результат выполнения запроса. </returns>
    /// <response code="200"> Успешно. </response>
    /// <response code="400"> Майнинг устройство не найдено. </response>
    [HttpGet("overclocking")]
    [ProducesResponseType(typeof(IOverclockingModel), 200)]
    [ProducesResponseType(typeof(List<string>), 400)]
    [WithoutLicenseChecking]
    public async Task<IActionResult> GetOverclocking(Guid deviceId)
    {
        var userId = _userAccessor.GetUserId();
        var result = await _mediator.Send(
            new GetDeviceOverclockingQuery(deviceId, userId));
        return result.ToActionResult();
    }

    /// <summary>
    /// Задать разгон устройству напрямую.
    /// </summary>
    /// <param name="deviceId"> Идентификатор устройства. </param>
    /// <param name="overclocking"> Разгон. </param>
    /// <returns> Результат выполнения запроса. </returns>
    /// <response code="200"> Успешно. </response>
    /// <response code="400">
    /// Девайса не существует или параметры разгона являются некорректными.
    /// </response>
    [HttpPost("overclocking")]
    [ProducesResponseType(typeof(Guid[]), 200)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> SetOverclocking(Guid deviceId,
                                                     IOverclockingModel overclocking)
    {
        var userId = _userAccessor.GetUserId();
        var result = await _mediator.Send(
            new SetOverclockingCommand(userId, overclocking, deviceId));
        return result.ToActionResult();
    }
}
