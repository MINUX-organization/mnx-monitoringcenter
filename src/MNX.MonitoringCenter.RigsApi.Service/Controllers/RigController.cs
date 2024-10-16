using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Infrastructure;
using MNX.MonitoringCenter.Inventory.Contracts;
using MNX.MonitoringCenter.Inventory.Contracts.Cpu;
using MNX.MonitoringCenter.Inventory.Contracts.Drive;
using MNX.MonitoringCenter.Inventory.Contracts.Gpu;
using MNX.MonitoringCenter.Inventory.Contracts.Motherboard;
using MNX.MonitoringCenter.Inventory.Contracts.NetworkAdapter;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.CountDevices;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Cpu;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Drive;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Gpu;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Motherboard;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.NetworkAdapter;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Software;
using MNX.MonitoringCenter.Inventory.Contracts.Rig;

namespace MNX.MonitoringCenter.RigsApi.Service.Controllers;

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
    /// Получить список ригов.
    /// </summary>
    /// <returns> Асинхронный поток ригов. </returns>
    [HttpGet]
    [ProducesResponseType(typeof(IAsyncEnumerable<Rig>), 200)]
    public IAsyncEnumerable<Rig> GetList()
    {
        var userId = _userAccessor.GetUserId();
        return _mediator.CreateStream(new GetRigsQuery(userId));
    }

    /// <summary>
    /// Получить обобщённые количественные данные.
    /// </summary>
    /// <returns> Результат получения обобщённых количественных данных. </returns>
    /// <response code="200"> Успешно </response>
    [HttpGet("total_data")]
    [ProducesResponseType(typeof(GetCountDevicesQueryResponse), 200)]
    public async Task<IActionResult> GetSummarizedQuantitativeData()
    {
        var userId = _userAccessor.GetUserId();
        var result = await _mediator.Send(new GetCountDevicesQuery(userId));
        return result.ToActionResult();
    }

    /// <summary>
    /// Получить кол-во устройств ( видеокарты, процессоры, диски )
    /// </summary>
    /// <param name="rigId"> Идентификатор рига. </param>
    /// <returns> Кол-во устройств на риге. </returns>
    /// <response code="200"> Успешно </response>
    [HttpGet("{rigId:Guid}/devices/count")]
    [ProducesResponseType(typeof(GetCountDevicesQueryResponse), 200)]
    public async Task<IActionResult> GetDevicesCount(Guid rigId)
    {
        var userId = _userAccessor.GetUserId();
        var result = await _mediator.Send(new GetCountDevicesQuery(userId, rigId));
        return result.ToActionResult();
    }

    /// <summary>
    /// Получить информацию о процессорах рига.
    /// </summary>
    /// <param name="rigId"> Идентификатор рига. </param>
    /// <returns> Результат выполнения запроса. </returns>
    /// <response code="200"> Успешно </response>
    [HttpGet("{rigId:Guid}/cpus")]
    [ProducesResponseType(typeof(IAsyncEnumerable<Cpu>), 200)]
    public IAsyncEnumerable<Cpu> GetCpus(Guid rigId)
    {
        var userId = _userAccessor.GetUserId();
        return _mediator.CreateStream(new GetCpusInfoQuery(userId, rigId));
    }

    /// <summary>
    /// Получить информацию о жёстких дисках рига
    /// </summary>
    /// <param name="rigId"> Идентификатор рига. </param>
    /// <returns> Результат выполнения запроса. </returns>
    /// <response code="200"> Успешно </response>
    /// <response code="400"> Запрашиваемые данные не были найдены. </response>
    [HttpGet("{rigId:Guid}/drives")]
    [ProducesResponseType(typeof(List<Drive>), 200)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> GetDrives(Guid rigId)
    {
        var userId = _userAccessor.GetUserId();
        var result = await _mediator.Send(new GetDrivesInfoQuery(userId, rigId));
        return result.ToActionResult();
    }

    /// <summary>
    /// Получить информацию о видеокартах рига
    /// </summary>
    /// <param name="rigId"> Идентификатор рига. </param>
    /// <returns> Результат выполнения запроса. </returns>
    /// <response code="200"> Успешно </response>
    [HttpGet("{rigId:Guid}/gpus")]
    [ProducesResponseType(typeof(IAsyncEnumerable<Gpu>), 200)]
    public IAsyncEnumerable<Gpu> GetGpus(Guid rigId)
    {
        var userId = _userAccessor.GetUserId();
        return _mediator.CreateStream(new GetGpusInfoQuery(userId, rigId));
    }

    /// <summary>
    /// Получить информацию о материнской плате рига
    /// </summary>
    /// <param name="rigId"> Идентификатор рига. </param>
    /// <returns> Результат выполнения запроса. </returns>
    /// <response code="200"> Успешно </response>
    /// <response code="400"> Запрашиваемые данные не были найдены. </response>
    [HttpGet("{rigId:Guid}/motherboard")]
    [ProducesResponseType(typeof(Motherboard), 200)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> GetMotherboard(Guid rigId)
    {
        var userId = _userAccessor.GetUserId();
        var result = await _mediator.Send(new GetMotherboardInfoQuery(userId, rigId));
        return result.ToActionResult();
    }

    /// <summary>
    /// Получить информацию об интернет соединении рига
    /// </summary>
    /// <param name="rigId"> Идентификатор рига. </param>
    /// <returns> Результат выполнения запроса. </returns>
    /// <response code="200"> Успешно </response>
    /// <response code="400"> Запрашиваемые данные не были найдены. </response>
    [HttpGet("{rigId:Guid}/network_adapters")]
    [ProducesResponseType(typeof(List<NetworkAdapter>), 200)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> GetNetworkAdapters(Guid rigId)
    {
        var userId = _userAccessor.GetUserId();
        var result = await _mediator.Send(new GetNetworkAdaptersInfoQuery(userId, rigId));
        return result.ToActionResult();
    }

    /// <summary>
    /// Получить информацию о программном обеспечении рига
    /// </summary>
    /// <param name="rigId"> Идентификатор рига. </param>
    /// <returns> Результат выполнения запроса. </returns>
    /// <response code="200"> Успешно </response>
    /// <response code="400"> Запрашиваемые данные не были найдены. </response>
    [HttpGet("{rigId:Guid}/software")]
    [ProducesResponseType(typeof(SoftwareInventory), 200)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> GetSoftware(Guid rigId)
    {
        var userId = _userAccessor.GetUserId();
        var result = await _mediator.Send(new GetSoftwareInfoQuery(userId, rigId));
        return result.ToActionResult();
    }
}
