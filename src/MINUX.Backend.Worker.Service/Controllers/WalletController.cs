using Kernel.UseCases;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MINUX.Backend.Worker.Core;
using MINUX.Backend.Worker.UseCases.Commands.Wallets;
using MINUX.Backend.Worker.UseCases.Commands.Wallets.AddWallet;
using MINUX.Backend.Worker.UseCases.Commands.Wallets.EditWallet;
using MINUX.Backend.Worker.UseCases.Commands.Wallets.RemoveWallet;
using MINUX.Backend.Worker.UseCases.Queries.GetWalletsQuery;

namespace MINUX.Backend.Worker.Controllers;

/// <summary>
/// Контроллер, предоставляющий Rest API для доступа к криптокошелькам
/// </summary>
[Route("api/wallet")]
[ApiController]
public class WalletController : ControllerBase
{
    private readonly IMediator _mediator;

    public WalletController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Получить список всех криптокошельков
    /// </summary>
    /// <returns> Список криптокошельков </returns>
    /// <response code="200"> Успешно </response>
    [HttpGet]
    [ProducesResponseType(typeof(IAsyncEnumerable<Preset>), 200)]
    public IAsyncEnumerable<Wallet> GetAll()
    {
        return _mediator.CreateStream(new GetWalletsQuery());
    }

    /// <summary>
    /// Добавить кошелёк
    /// </summary>
    /// <param name="model"> Модель кошелька </param>
    /// <returns> Результат выполнения операции </returns>
    /// <response code="201"> Успешно </response>
    /// <response code="400">
    /// Переданные параметры не прошли валидацию или не была найдена монета с указанным названием
    /// </response>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), 201)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> Add(WalletModel model)
    {
        var result = await _mediator.Send(new AddWalletCommand(model));
        return result.ToActionResult();
    }

    /// <summary>
    /// Редактировать кошелёк
    /// </summary>
    /// <param name="id"> Уникальный идентификатор </param>
    /// <param name="model"> Модель кошелька </param>
    /// <returns> Результат выполенениия операции </returns>
    /// <response code="204"> Успешно </response>
    /// <response code="400">
    /// Переданные параметры не прошли валидацию или не был найден кошелёк с переданным id
    /// </response>
    [HttpPut("{id:Guid}")]
    [ProducesResponseType(typeof(Guid), 204)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> Edit(Guid id, WalletModel model)
    {
        var result = await _mediator.Send(new EditWalletCommand(id, model));
        return result.ToActionResult();
    }

    /// <summary>
    /// Удалить кошелёк
    /// </summary>
    /// <param name="id"> Уникальный идентификатор </param>
    /// <returns> Результат выполнения операции </returns>
    /// <response code="204"> Успешно </response>
    /// <response code="400"> Не был найден кошелёк с переданным идентификатором </response>
    [HttpDelete("{id:Guid}")]
    [ProducesResponseType(typeof(Guid), 204)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new RemoveWalletCommand(id));
        return result.ToActionResult();
    }
}