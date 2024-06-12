using MediatR;
using Microsoft.AspNetCore.Mvc;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Infrastructure;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets.RemovePreset;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets.SavePreset;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets.UpdatePreset;
using MNX.MonitoringCenter.Management.UseCases.Queries.GetPresetsQuery;
using Microsoft.AspNetCore.Authorization;
using MNX.MonitoringCenter.Management.Contracts;

namespace MNX.MonitoringCenter.Management.Controllers;

/// <summary>
/// Предоставляет REST API для работы с пресетами
/// </summary>
[Route("api/presets")]
[ApiController]
//[Authorize]
public class PresetController : ControllerBase
{
    /// <summary>
    /// Медиатор.
    /// </summary>
    private readonly IMediator _mediator;

    /// <summary>
    /// Сервис для доступа к данным пользователя.
    /// </summary>
    private readonly UserAccessor _userAccessor;

    public PresetController(IMediator mediator, UserAccessor userAccessor)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _userAccessor = userAccessor ?? throw new ArgumentNullException(nameof(userAccessor));
    }

    /// <summary>
    /// Получить список пресетов
    /// </summary>
    /// <remarks>
    /// Если указано название GPU, возвращается список всех пресетов для данной серии видеокарт,
    /// иначе возвращаются все существующие пресеты.
    /// </remarks>
    /// <param name="gpuName"> Название GPU </param>
    /// <returns> Список пресетов </returns>
    /// <response code="200"> Успешно </response>
    [HttpGet]
    [ProducesResponseType(typeof(IAsyncEnumerable<PresetModel>), 200)]
    public IAsyncEnumerable<PresetModel> GetPresets(string? gpuName)
    {
        var userId = 1;// _userAccessor.GetUserId();
        return _mediator.CreateStream(new GetPresetsQuery(gpuName, userId));
    }

    /// <summary>
    /// Сохранить пресет
    /// </summary>
    /// <param name="model"> Входная модель пресета </param>
    /// <returns> Результат выполнения команды </returns>
    /// <response code="201"> Успешно </response>
    /// <response code="400">
    /// Переданные параметры не прошли валидацию или не была найдена GPU с указанным названием
    /// </response>
    [HttpPost]
    [ProducesResponseType(typeof(PresetModel), 201)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> Save(SavePresetInputModel model)
    {
        var userId = 1;// _userAccessor.GetUserId();
        var result = await _mediator.Send(new SavePresetCommand(userId, model));
        return result.ToActionResult();
    }

    /// <summary>
    /// Обновить пресет
    /// </summary>
    /// <param name="id"> Уникальный идентификатор </param>
    /// <param name="model"> Входная модель пресета </param>
    /// <returns> Результат выполнения команды </returns>
    /// <response code="204"> Успешно </response>
    /// <response code="400">
    /// Переданные параметры не прошли валидацию или не был найден пресет с переданным id
    /// </response>
    [HttpPut("{id:Guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> Update(Guid id, SavePresetInputModel model)
    {
        var userId = 1;// _userAccessor.GetUserId();
        var result = await _mediator.Send(new UpdatePresetCommand(id, model, userId));
        return result.ToActionResult();
    }

    /// <summary>
    /// Удалить пресет
    /// </summary>
    /// <param name="id"> Уникальный идентификатор </param>
    /// <returns> Результат выполнения команды </returns>
    /// <response code="204"> Успешно </response>
    /// <response code="400"> Не был найден пресет с переданным идентификатором </response>
    [HttpDelete("{id:Guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> Remove(Guid id)
    {
        var userId = 1;// _userAccessor.GetUserId();
        var result = await _mediator.Send(new RemovePresetCommand(id, userId));
        return result.ToActionResult();
    }
}
