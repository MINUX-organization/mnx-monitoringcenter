using Kernel.UseCases;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Commands.Wallets;
using MNX.MonitoringCenter.Management.UseCases.Commands.Wallets.AddWallet;
using MNX.MonitoringCenter.Management.UseCases.Commands.Wallets.EditWallet;
using MNX.MonitoringCenter.Management.UseCases.Commands.Wallets.RemoveWallet;
using MNX.MonitoringCenter.Management.UseCases.Queries.GetWalletsQuery;

namespace MNX.MonitoringCenter.Management.Controllers;

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
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// Получить список всех криптокошельков
    /// </summary>
    /// <returns> Список криптокошельков </returns>
    /// <response code="200"> Успешно </response>
    [HttpGet]
    [ProducesResponseType(typeof(IAsyncEnumerable<WalletModel>), 200)]
    public IAsyncEnumerable<WalletModel> GetAll()
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
    public async Task<IActionResult> Add(WalletInputModel model)
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
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> Edit(Guid id, WalletInputModel model)
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
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new RemoveWalletCommand(id));
        return result.ToActionResult();
    }
}