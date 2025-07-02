using MediatR;
using Microsoft.AspNetCore.Mvc;
using MNX.Application.UseCases.Results;
using Microsoft.AspNetCore.Authorization;
using MNX.MonitoringCenter.Management.Core.Mining;
using MNX.MonitoringCenter.RigsApi.Service.Infrastructure;
using MNX.MonitoringCenter.Management.Contracts.AlgorithmBinding;
using MNX.MonitoringCenter.Management.UseCases.Mining.Algorithm.Queries;
using MNX.MonitoringCenter.Management.UseCases.Mining.Algorithm.Queries.GetAlgorithmById;
using MNX.MonitoringCenter.Management.UseCases.Mining.Algorithm.Commands.AddAlgorithmCommand;
using MNX.MonitoringCenter.Management.UseCases.Mining.Algorithm.Commands.DeleteAlgorithmCommand;
using MNX.MonitoringCenter.Management.UseCases.Mining.Algorithm.Commands.EditAlgorithmNameAndMinerAlgorithmsCommand;

namespace MNX.MonitoringCenter.RigsApi.Service.Controllers.Management;

/// <summary>
/// Содержит эндпоинты, связанные с алгоритмами.
/// </summary>
[Route("api/algorithms")]
[ApiController]
[Authorize]
public class AlgorithmController : ControllerBase
{
    /// <summary>
    /// Медиатор.
    /// </summary>
    private readonly IMediator _mediator;

    /// <summary>
    /// Сервис для доступ к данным пользователя.
    /// </summary>
    private readonly UserAccessor _accessor;

    public AlgorithmController(IMediator mediator, UserAccessor accessor)
    {
        _mediator = mediator
            ?? throw new ArgumentNullException(nameof(mediator));
        _accessor = accessor
            ?? throw new ArgumentNullException(nameof(accessor));
    }

    /// <summary>
    /// Получить список доступных алгоритмов.
    /// </summary>
    /// <returns> Список доступных алгоритмов. </returns>
    /// <response code="200"> Успешно. </response>
    [HttpGet("available")]
    [ProducesResponseType(typeof(IAsyncEnumerable<Algorithm>), 200)]
    public IAsyncEnumerable<Algorithm> GetAvailable()
    {
        return _mediator.CreateStream(
            new GetAvailableAlgorithmsQuery(_accessor.GetUserId()));
    }

    /// <summary>
    /// Получить алгоритм по идентификатору.
    /// </summary>
    /// <param name="id"> Идентификатор алгоритма. </param>
    /// <returns> Алгоритм с относительными 
    /// наименованиями для майнеров. </returns>
    /// <response code="200"> Успешно. </response>
    /// <response code="400"> Алгоритм не найден. </response>
    [HttpGet("{id:Guid}")]
    [ProducesResponseType(typeof(AlgorithmBindingModel), 200)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(
            new GetAlgorithmByIdQuery(id, _accessor.GetUserId()));
        return result.ToActionResult();
    }

    /// <summary>
    /// Добавить пользовательский алгоритм.
    /// </summary>
    /// <param name="model"> Модель пользовательского алгоритма. </param>
    /// <response code="201"> Успешно. </response>
    /// <response code="400">
    /// Переданные параметры не прошли валидацию 
    /// или не был найден алгоритм.
    /// </response>
    /// <response code="409"> 
    /// Алгоритм с таким наименованием уже существует.
    /// </response>
    [HttpPost]
    [ProducesResponseType(typeof(AlgorithmBindingModel), 201)]
    [ProducesResponseType(typeof(List<string>), 400)]
    [ProducesResponseType(typeof(List<string>), 409)]
    public async Task<IActionResult> AddAlgorithm(AlgorithmBindingModel model)
    {
        var result = await _mediator.Send(
            new AddAlgorithmCommand(_accessor.GetUserId(), model));
        return result.ToActionResult();
    }

    /// <summary>
    /// Редактировать пользовательский алгоритм по идентификатору.
    /// </summary>
    /// <param name="id"> Идентификатор алгоритма. </param>
    /// <param name="model"> Новый пользовательский алгоритм. </param>
    /// <response code="200"> Успешно. </response>
    /// <response code="400">
    /// Переданные параметры не прошли валидацию,
    /// не был найден алгоритм с переданным id или алгоритм 
    /// является доменным и не может быть отредактирован.
    /// </response>
    /// <response code="409"> 
    /// Алгоритм с таким наименованием уже существует.
    /// </response>
    [HttpPut("{id:Guid}")]
    [ProducesResponseType(typeof(AlgorithmBindingModel), 200)]
    [ProducesResponseType(typeof(List<string>), 400)]
    [ProducesResponseType(typeof(List<string>), 409)]
    public async Task<IActionResult> EditAlgorithm(Guid id,
                                                   AlgorithmBindingModel model)
    {
        var result = await _mediator.Send(
            new EditAlgorithmAndMinerAlgorithmsCommand(id, _accessor.GetUserId(), model));
        return result.ToActionResult();
    }

    /// <summary>
    /// Удалить пользовательский алгоритм по идентификатору.
    /// </summary>
    /// <param name="id"> Идентификатор алгоритма. </param>
    /// <response code="204"> Успешно. </response>
    /// <response code="400"> Алгоритм не может быть удален. </response>
    [HttpDelete("{id:Guid}")]
    [ProducesResponseType(typeof(AlgorithmBindingModel), 204)]
    [ProducesResponseType(typeof(AlgorithmBindingModel), 400)]
    public async Task<IActionResult> DeleteAlgorithm(Guid id)
    {
        var result = await _mediator.Send(
            new DeleteAlgorithmCommand(id, _accessor.GetUserId()));
        return result.ToActionResult();
    }
}