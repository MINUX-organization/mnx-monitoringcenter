using MediatR;
using Microsoft.AspNetCore.Mvc;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Infrastructure;
using MNX.MonitoringCenter.Management.UseCases.Commands.Pools;
using MNX.MonitoringCenter.Management.UseCases.Commands.Pools.AddPool;
using MNX.MonitoringCenter.Management.UseCases.Commands.Pools.RemovePool;
using MNX.MonitoringCenter.Management.UseCases.Commands.Pools.UpdatePool;
using MNX.MonitoringCenter.Management.UseCases.Queries.GetPoolsQuery;
using Microsoft.AspNetCore.Authorization;

namespace MNX.MonitoringCenter.Management.Controllers;

/// <summary>
/// Контроллер предоставляющий Rest API для работы с пулами
/// </summary>
[Route("api/pools")]
[ApiController]
[Authorize]
public class PoolController : ControllerBase
{
    /// <summary>
    /// Медиатор.
    /// </summary>
    private readonly IMediator _mediator;

    /// <summary>
    /// Сервис для доступа к данным пользователя.
    /// </summary>
    private readonly UserAccessor _userAccessor;

    public PoolController(IMediator mediator, UserAccessor userAccessor)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _userAccessor = userAccessor ?? throw new ArgumentNullException(nameof(userAccessor));
    }

    /// <summary>
    /// Получить список всех пулов
    /// </summary>
    /// <returns> Список пулов </returns>
    /// <response code="200"> Успешно </response>
    [HttpGet]
    [ProducesResponseType(typeof(IAsyncEnumerable<PoolModel>), 200)]
    public IAsyncEnumerable<PoolModel> GetAll()
    {
        var userId = _userAccessor.GetUserId();
        return _mediator.CreateStream(new GetPoolsQuery(userId));
    }

    /// <summary>
    /// Добавить пул
    /// </summary>
    /// <param name="model"> Входная модель пула </param>
    /// <returns> Результат выполнения операции </returns>
    /// <response code="201"> Успешно </response>
    /// <response code="400">
    /// Переданные параметры не прошли валидацию или не была найдена монета с указанным названием
    /// </response>
    [HttpPost]
    [ProducesResponseType(typeof(PoolModel), 201)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> Add(PoolInputModel model)
    {
        var userId = _userAccessor.GetUserId();
        var result = await _mediator.Send(new AddPoolCommand(model, userId));
        return result.ToActionResult();
    }

    /// <summary>
    /// Обновить пул
    /// </summary>
    /// <param name="id"> Уникальный идентификатор </param>
    /// <param name="model"> Входная модель пула </param>
    /// <returns> Результат выполнения операции </returns>
    /// <response code="200"> Успешно </response>
    /// <response code="400">
    /// Переданные параметры не прошли валидацию или не был найден пул с переданным id
    /// </response>
    [HttpPut("{id:Guid}")]
    [ProducesResponseType(typeof(PoolModel), 200)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> Update(Guid id, PoolInputModel model)
    {
        var userId = _userAccessor.GetUserId();
        var result = await _mediator.Send(new UpdatePoolCommand(id, model, userId));
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
        var userId = _userAccessor.GetUserId();
        var result = await _mediator.Send(new RemovePoolCommand(id, userId));
        return result.ToActionResult();
    }
}
