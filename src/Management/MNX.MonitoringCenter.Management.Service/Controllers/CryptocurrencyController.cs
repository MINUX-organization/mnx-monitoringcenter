using Kernel.UseCases;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Commands.Crypto.AddCryptocurrency;
using MNX.MonitoringCenter.Management.UseCases.Commands.Crypto.RemoveCryptocurrency;
using MNX.MonitoringCenter.Management.UseCases.Queries.GetCryptocurrenciesQuery;

namespace MNX.MonitoringCenter.Management.Controllers;

/// <summary>
/// Контроллер, предоставляющий Rest API для работы с криптовалютой
/// </summary>
[Route("api/cryptocurrency")]
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
        return _mediator.CreateStream(new GetCryptocurrenciesQuery());
    }

    /// <summary>
    /// Добавить криптовалюту
    /// </summary>
    /// <param name="request"> Команда добавления криптовалюты </param>
    /// <response code="201"> Успешно </response>
    /// <response code="400">
    /// Переданные параметры не прошли валидацию или не был найден алгоритм с указанным названием
    /// </response>
    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> Add(AddCryptocurrencyCommand request)
    {
        var result = await _mediator.Send(request);
        return result.ToActionResult();
    }

    /// <summary>
    /// Удалить криптовалюту
    /// </summary>
    /// <param name="fullName"> Полное название криптовалюты </param>
    /// <response code="204"> Успешно </response>
    /// <response code="400"> Не была найдена монета с переданным именем </response>
    [HttpDelete("{fullName}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> Delete(string fullName)
    {
        var result = await _mediator.Send(new RemoveCryptocurrencyCommand(fullName));
        return result.ToActionResult();
    }
}
