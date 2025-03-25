using MediatR;
using Microsoft.AspNetCore.Mvc;
using MNX.Application.UseCases.Results;
using Microsoft.AspNetCore.Authorization;
using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.RigsApi.Service.Infrastructure;
using MNX.MonitoringCenter.Management.UseCases.Mining.Pool.Queries;
using MNX.MonitoringCenter.Management.UseCases.Mining.Pool.Commands;
using MNX.MonitoringCenter.Management.UseCases.Mining.Pool.Commands.AddPool;
using MNX.MonitoringCenter.Management.UseCases.Mining.Pool.Commands.EditPool;
using MNX.MonitoringCenter.Management.UseCases.Mining.Pool.Commands.RemovePool;

namespace MNX.MonitoringCenter.RigsApi.Service.Controllers.Management;

/// <summary>
/// Контроллер предоставляющий Rest API для работы с пулами.
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
    /// Получить список всех пулов.
    /// </summary>
    /// <returns> Список пулов. </returns>
    /// <response code="200"> Успешно. </response>
    [HttpGet]
    [ProducesResponseType(typeof(IAsyncEnumerable<PoolModel>), 200)]
    public IAsyncEnumerable<PoolModel> GetAll()
    {
        var userId = _userAccessor.GetUserId();
        return _mediator.CreateStream(new GetPoolsQuery(userId));
    }

    /// <summary>
    /// Добавить пул.
    /// </summary>
    /// <param name="model"> Входная модель пула. </param>
    /// <returns> Результат выполнения операции. </returns>
    /// <response code="201"> Успешно. </response>
    /// <response code="400">
    /// Переданные параметры не прошли валидацию или не была найдена монета с указанным названием.
    /// </response>
    /// <response code="409"> Пул уже существует. </response>
    [HttpPost]
    [ProducesResponseType(typeof(PoolModel), 201)]
    [ProducesResponseType(typeof(List<string>), 400)]
    [ProducesResponseType(typeof(List<string>), 409)]
    public async Task<IActionResult> Add(PoolInputModel model)
    {
        var userId = _userAccessor.GetUserId();
        var result = await _mediator.Send(new AddPoolCommand(model, userId));
        return result.ToActionResult();
    }

    /// <summary>
    /// Редактировать пул.
    /// </summary>
    /// <param name="id"> Уникальный идентификатор. </param>
    /// <param name="model"> Входная модель пула. </param>
    /// <returns> Результат выполнения операции. </returns>
    /// <response code="200"> Успешно. </response>
    /// <response code="400">
    /// Переданные параметры не прошли валидацию или не был найден пул с переданным id.
    /// </response>
    /// <response code="409"> Пул уже существует. </response>
    [HttpPut("{id:Guid}")]
    [ProducesResponseType(typeof(PoolModel), 200)]
    [ProducesResponseType(typeof(List<string>), 400)]
    [ProducesResponseType(typeof(List<string>), 409)]
    public async Task<IActionResult> Edit(Guid id, PoolInputModel model)
    {
        var userId = _userAccessor.GetUserId();
        var result = await _mediator.Send(new EditPoolCommand(id, model, userId));
        return result.ToActionResult();
    }

    /// <summary>
    /// Удалить пул.
    /// </summary>
    /// <param name="id"> Уникальный идентификатор. </param>
    /// <returns> Результат выполнения операции. </returns>
    /// <response code="204"> Успешно. </response>
    [HttpDelete("{id:Guid}")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = _userAccessor.GetUserId();
        var result = await _mediator.Send(new RemovePoolCommand(id, userId));
        return result.ToActionResult();
    }
}
