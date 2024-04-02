using Kernel.UseCases;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Commands.Crypto;
using MNX.MonitoringCenter.Management.UseCases.Commands.Crypto.AddCryptocurrency;
using MNX.MonitoringCenter.Management.UseCases.Commands.Crypto.RemoveCryptocurrency;
using MNX.MonitoringCenter.Management.UseCases.Queries.GetCryptocurrenciesQuery;

namespace MNX.MonitoringCenter.Management.Controllers;

/// <summary>
/// Контроллер, предоставляющий Rest API для работы с криптовалютой
/// </summary>
[Route("api/cryptocurrencies")]
[ApiController]
public class CryptocurrencyController : ControllerBase
{
    private readonly IMediator _mediator;

    public CryptocurrencyController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// Получить список всех криптовалют
    /// </summary>
    /// <returns> Список криптовалют </returns>
    /// <response code="200"> Успешно </response>
    [HttpGet]
    [ProducesResponseType(typeof(IAsyncEnumerable<Cryptocurrency>), 200)]
    public IAsyncEnumerable<Cryptocurrency> GetAll()
    {
        var userId = 1;
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
    [ProducesResponseType(typeof(Cryptocurrency), 201)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> Add(CryptocurrencyInputModel model)
    {
        var userId = 1;
        var result = await _mediator.Send(new AddCryptocurrencyCommand(model, userId));
        return result.ToActionResult();
    }

    /// <summary>
    /// Удалить криптовалюту
    /// </summary>
    /// <param name="id"> Идентификатор криптовалюты </param>
    /// <response code="204"> Успешно </response>
    /// <response code="400"> Не была найдена монета с переданным идентификатором </response>
    [HttpDelete("{id:Guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = 1;
        var result = await _mediator.Send(new RemoveCryptocurrencyCommand(id, userId));
        return result.ToActionResult();
    }
}
