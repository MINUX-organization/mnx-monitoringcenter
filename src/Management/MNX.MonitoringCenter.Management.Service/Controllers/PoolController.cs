using MediatR;
using Microsoft.AspNetCore.Mvc;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Queries.GetPoolsQuery;
using Kernel.UseCases;
using MNX.MonitoringCenter.Management.UseCases.Commands.Pools.AddPool;
using MNX.MonitoringCenter.Management.UseCases.Commands.Pools;
using MNX.MonitoringCenter.Management.UseCases.Commands.Pools.RemovePool;
using MNX.MonitoringCenter.Management.UseCases.Commands.Pools.UpdatePool;

namespace MNX.MonitoringCenter.Management.Controllers;

/// <summary>
/// Контроллер предоставляющий Rest API для работы с пулами
/// </summary>
[Route("api/pool")]
[ApiController]
public class PoolController : ControllerBase
{
    private readonly IMediator _mediator;

    public PoolController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// Получить список всех пулов
    /// </summary>
    /// <returns> Список пулов </returns>
    /// <response code="200"> Успешно </response>
    [HttpGet]
    [ProducesResponseType(typeof(IAsyncEnumerable<Pool>), 200)]
    public IAsyncEnumerable<Pool> GetAll()
    {
        return _mediator.CreateStream(new GetPoolsQuery());
    }

    /// <summary>
    /// Добавить пул
    /// </summary>
    /// <param name="request"> Модкль пула </param>
    /// <returns> Результат выполнения операции </returns>
    /// <response code="201"> Успешно </response>
    /// <response code="400">
    /// Переданные параметры не прошли валидацию или не была найдена монета с указанным названием
    /// </response>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), 201)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> Add(PoolModel request)
    {
        var result = await _mediator.Send(new AddPoolCommand(request));
        return result.ToActionResult();
    }

    /// <summary>
    /// Обновить пул
    /// </summary>
    /// <param name="id"> Уникальный идентификатор </param>
    /// <param name="request"> Модель пула </param>
    /// <returns> Результат выполнения операции </returns>
    /// <response code="204"> Успешно </response>
    /// <response code="400">
    /// Переданные параметры не прошли валидацию или не был найден пул с переданным id
    /// </response>
    [HttpPut("{id:Guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> Update(Guid id, PoolModel request)
    {
        var result = await _mediator.Send(new UpdatePoolCommand(id, request));
        return result.ToActionResult();
    }

    /// <summary>
    /// Удалить пул
    /// </summary>
    /// <param name="id"> Уникальный идентификатор </param>
    /// <returns> Результат выполнения операции </returns>
    /// <response code="204"> Успешно </response>
    /// <response code="400"> Не был найден пул с переданным идентификатором </response>
    [HttpDelete("{id:Guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new RemovePoolCommand(id));
        return result.ToActionResult();
    }
}
