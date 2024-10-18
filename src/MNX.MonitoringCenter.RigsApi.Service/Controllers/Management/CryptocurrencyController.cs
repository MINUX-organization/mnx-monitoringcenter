using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Management.UseCases.Cryptocurrency.Commands;
using MNX.MonitoringCenter.Management.UseCases.Cryptocurrency.Commands.AddCryptocurrency;
using MNX.MonitoringCenter.Management.UseCases.Cryptocurrency.Commands.RemoveCryptocurrency;
using MNX.MonitoringCenter.Management.UseCases.Cryptocurrency.Queries;
using MNX.MonitoringCenter.RigsApi.Service.Infrastructure;

namespace MNX.MonitoringCenter.RigsApi.Service.Controllers.Management;

/// <summary>
/// Контроллер, предоставляющий Rest API для работы с криптовалютой
/// </summary>
[Route("api/cryptocurrencies")]
[ApiController]
[Authorize]
public class CryptocurrencyController : ControllerBase
{
    /// <summary>
    /// Медиатор.
    /// </summary>
    private readonly IMediator _mediator;

    /// <summary>
    /// Сервис для доступа к данным пользователя.
    /// </summary>
    private readonly UserAccessor _userAccessor;

    public CryptocurrencyController(IMediator mediator, UserAccessor userAccessor)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _userAccessor = userAccessor ?? throw new ArgumentNullException(nameof(userAccessor));
    }

    /// <summary>
    /// Получить список всех криптовалют
    /// </summary>
    /// <returns> Список криптовалют </returns>
    /// <response code="200"> Успешно </response>
    [HttpGet]
    [ProducesResponseType(typeof(IAsyncEnumerable<CryptocurrencyModel>), 200)]
    public IAsyncEnumerable<CryptocurrencyModel> GetAll()
    {
        var userId = _userAccessor.GetUserId();
        return _mediator.CreateStream(new GetCryptocurrenciesQuery(userId));
    }

    /// <summary>
    /// Добавить криптовалюту
    /// </summary>
    /// <param name="model"> Входная модель криптовалюты </param>
    /// <response code="201"> Успешно </response>
    /// <response code="400">
    /// Переданные параметры не прошли валидацию или не был найден алгоритм с указанным названием
    /// </response>
    [HttpPost]
    [ProducesResponseType(typeof(CryptocurrencyModel), 201)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> Add(CryptocurrencyInputModel model)
    {
        var userId = _userAccessor.GetUserId();
        var result = await _mediator.Send(new AddCryptocurrencyCommand(model, userId));
        return result.ToActionResult();
    }

    /// <summary>
    /// Удалить криптовалюту
    /// </summary>
    /// <param name="id"> Идентификатор криптовалюты </param>
    /// <response code="204"> Успешно </response>
    [HttpDelete("{id:Guid}")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = _userAccessor.GetUserId();
        var result = await _mediator.Send(new RemoveCryptocurrencyCommand(id, userId));
        return result.ToActionResult();
    }
}
