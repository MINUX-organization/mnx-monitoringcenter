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
using MNX.MonitoringCenter.Inventory.UseCases.Cpu;
using MNX.MonitoringCenter.Inventory.UseCases.Drive;
using MNX.MonitoringCenter.Inventory.UseCases.Gpu;
using MNX.MonitoringCenter.Inventory.UseCases.Motherboard;
using MNX.MonitoringCenter.Inventory.UseCases.NetworkAdapter;
using MNX.MonitoringCenter.Inventory.UseCases.Software;

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

    /*
    /// <summary>
    /// Получить информацию о ригах.
    /// </summary>
    /// <returns> Информация о ригах. </returns>
    /// <response code="200"> Успешно </response>
    [HttpGet("info")]
    [ProducesResponseType(typeof(IAsyncEnumerable<RigInformationMessage>), 200)]
    public IAsyncEnumerable<RigInformationMessage> GetRigsInfo(string? searchString = null,
                                                               string? filter = null,
                                                               [FromQuery] string[]? filterParameters = null)
    {
        var userId = _userAccessor.GetUserId();
        return _mediator.CreateStream(
            new GetRigsInformationQuery(userId, searchString, filter, filterParameters));
    }

    /// <summary>
    /// Получить обобщённые количественные данные.
    /// </summary>
    /// <returns> Результат получения обобщённых количественных данных. </returns>
    /// <response code="200"> Успешно </response>
    [HttpGet("total_data")]
    [ProducesResponseType(typeof(RigsSummarizedQuantitativeData), 200)]
    public async Task<IActionResult> GetSummarizedQuantitativeData()
    {
        var userId = _userAccessor.GetUserId();
        var result = await _mediator.Send(new GetRigsSummarizedQuantitativeDataQuery(userId));
        return result.ToActionResult();
    }
    */

    /// <summary>
    /// Получить информацию о процессорах рига.
    /// </summary>
    /// <param name="rigId"> Идентификатор рига. </param>
    /// <returns> Результат выполнения запроса. </returns>
    /// <response code="200"> Успешно </response>
    [HttpGet("{rigId}/cpus")]
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
    [HttpGet("{rigId}/drives")]
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
    [HttpGet("{rigId}/gpus")]
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
    [HttpGet("{rigId}/motherboard")]
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
    [HttpGet("{rigId}/network_adapters")]
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
    [HttpGet("{rigId}/software")]
    [ProducesResponseType(typeof(SoftwareInventory), 200)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> GetSoftware(Guid rigId)
    {
        var userId = _userAccessor.GetUserId();
        var result = await _mediator.Send(new GetSoftwareInfoQuery(userId, rigId));
        return result.ToActionResult();
    }
}
