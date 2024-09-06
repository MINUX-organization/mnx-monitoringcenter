using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Infrastructure;
using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Management.UseCases.Wallet.Commands;
using MNX.MonitoringCenter.Management.UseCases.Wallet.Commands.AddWallet;
using MNX.MonitoringCenter.Management.UseCases.Wallet.Commands.EditWallet;
using MNX.MonitoringCenter.Management.UseCases.Wallet.Commands.RemoveWallet;
using MNX.MonitoringCenter.Management.UseCases.Wallet.Queries;

namespace MNX.MonitoringCenter.Management.Controllers;

/// <summary>
/// Контроллер, предоставляющий Rest API для доступа к криптокошелькам
/// </summary>
[Route("api/wallets")]
[ApiController]
[Authorize]
public class WalletController : ControllerBase
{
    /// <summary>
    /// Медиатор.
    /// </summary>
    private readonly IMediator _mediator;

    /// <summary>
    /// Профиль аутентифицированного пользователя.
    /// </summary>
    private readonly UserAccessor _userAccessor;

    public WalletController(IMediator mediator, UserAccessor userAccessor)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _userAccessor = userAccessor ?? throw new ArgumentNullException(nameof(userAccessor));
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
        var userId = _userAccessor.GetUserId();
        return _mediator.CreateStream(new GetWalletsQuery(userId));
    }

    /// <summary>
    /// Добавить кошелёк
    /// </summary>
    /// <param name="model"> Входная модель кошелька </param>
    /// <returns> Результат выполнения операции </returns>
    /// <response code="201"> Успешно </response>
    /// <response code="400">
    /// Переданные параметры не прошли валидацию или не была найдена монета с указанным названием
    /// </response>
    [HttpPost]
    [ProducesResponseType(typeof(WalletModel), 201)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> Add(WalletInputModel model)
    {
        var userId = _userAccessor.GetUserId();
        var result = await _mediator.Send(new AddWalletCommand(model, userId));
        return result.ToActionResult();
    }

    /// <summary>
    /// Редактировать кошелёк
    /// </summary>
    /// <param name="id"> Уникальный идентификатор </param>
    /// <param name="model"> Входная модель кошелька </param>
    /// <returns> Результат выполнения операции </returns>
    /// <response code="200"> Успешно </response>
    /// <response code="400">
    /// Переданные параметры не прошли валидацию или не был найден кошелёк с переданным id
    /// </response>
    [HttpPut("{id:Guid}")]
    [ProducesResponseType(typeof(WalletModel), 200)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> Edit(Guid id, WalletInputModel model)
    {
        var userId = _userAccessor.GetUserId();
        var result = await _mediator.Send(new EditWalletCommand(id, model, userId));
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
        var userId = _userAccessor.GetUserId();
        var result = await _mediator.Send(new RemoveWalletCommand(id, userId));
        return result.ToActionResult();
    }
}