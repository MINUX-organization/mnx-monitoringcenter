using MediatR;
using Microsoft.AspNetCore.Mvc;
using MNX.Application.UseCases.Results;
using Microsoft.AspNetCore.Authorization;
using MNX.MonitoringCenter.Management.Contracts.Presets;
using MNX.MonitoringCenter.RigsApi.Service.Infrastructure;
using MNX.MonitoringCenter.Management.UseCases.Overclocking.Presets.Queries;
using MNX.MonitoringCenter.Management.UseCases.Overclocking.Presets.Commands;
using MNX.MonitoringCenter.Management.UseCases.Overclocking.Presets.Commands.SavePreset;
using MNX.MonitoringCenter.Management.UseCases.Overclocking.Presets.Commands.EditPreset;
using MNX.MonitoringCenter.Management.UseCases.Overclocking.Presets.Commands.ApplyPreset;
using MNX.MonitoringCenter.Management.UseCases.Overclocking.Presets.Commands.RemovePreset;

namespace MNX.MonitoringCenter.RigsApi.Service.Controllers.Management;

/// <summary>
/// Предоставляет REST API для работы с пресетами.
/// </summary>
[Route("api/presets")]
[ApiController]
[Authorize]
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
    /// Получить список пресетов.
    /// </summary>
    /// <remarks>
    /// Если указано название GPU, возвращается список всех пресетов для данной серии видеокарт,
    /// иначе возвращаются все существующие пресеты.
    /// </remarks>
    /// <param name="gpuName"> Название GPU. </param>
    /// <returns> Список пресетов. </returns>
    /// <response code="200"> Успешно. </response>
    [HttpGet]
    [ProducesResponseType(typeof(IAsyncEnumerable<PresetModel>), 200)]
    public IAsyncEnumerable<PresetModel> GetPresets(string? gpuName)
    {
        var userId = _userAccessor.GetUserId();
        return _mediator.CreateStream(new GetPresetsQuery(gpuName, userId));
    }

    /// <summary>
    /// Получить пресет по его идентификатору.
    /// </summary>
    /// <param name="id"> Идентификатор пресета. </param>
    /// <returns> Пресет. </returns>
    /// <response code="200"> Успешно. </response>
    /// <response code="400"> Пресета с переданным идентификатором не существует. </response>
    [HttpGet("{id:Guid}")]
    [ProducesResponseType(typeof(PresetModel), 200)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> GetPresetById(Guid id)
    {
        var userId = _userAccessor.GetUserId();
        var result = await _mediator.Send(new GetPresetByIdQuery(id, userId));
        return result.ToActionResult();
    }

    /// <summary>
    /// Получить список пресетов, сгруппированных по названию видеокарт.
    /// </summary>
    /// <returns> Список сгруппированных пресетов. </returns>
    /// <response code="200"> Успешно. </response>
    [HttpGet("gpu_groups")]
    [ProducesResponseType(typeof(IAsyncEnumerable<PresetGroup>), 200)]
    public IAsyncEnumerable<PresetGroup> GetPresetsGroupedByGpuName()
    {
        var userId = _userAccessor.GetUserId();
        return _mediator.CreateStream(new GetPresetsGroupedByGpuNameQuery(userId));
    }

    /// <summary>
    /// Сохранить пресет.
    /// </summary>
    /// <param name="model"> Входная модель пресета. </param>
    /// <returns> Результат выполнения команды. </returns>
    /// <response code="201"> Успешно. </response>
    /// <response code="400">
    /// Переданные параметры не прошли валидацию или не была найдена GPU с указанным названием.
    /// </response>
    /// <response code="409"> Пресет с переданным именем уже существует. </response>
    [HttpPost]
    [ProducesResponseType(typeof(PresetModel), 201)]
    [ProducesResponseType(typeof(List<string>), 400)]
    [ProducesResponseType(typeof(List<string>), 409)]
    public async Task<IActionResult> Save(PresetInputModel model)
    {
        var userId = _userAccessor.GetUserId();
        var result = await _mediator.Send(new SavePresetCommand(userId, model));
        return result.ToActionResult();
    }

    /// <summary>
    /// Редактировать пресет.
    /// </summary>
    /// <param name="id"> Уникальный идентификатор. </param>
    /// <param name="model"> Входная модель пресета. </param>
    /// <returns> Результат выполнения команды. </returns>
    /// <response code="200"> Успешно. </response>
    /// <response code="400">
    /// Переданные параметры не прошли валидацию или не был найден пресет с переданным id.
    /// </response>
    /// <response code="409"> Пресет с переданным именем уже существует. </response>
    [HttpPatch("{id:Guid}")]
    [ProducesResponseType(typeof(PresetModel), 200)]
    [ProducesResponseType(typeof(List<string>), 400)]
    [ProducesResponseType(typeof(List<string>), 409)]
    public async Task<IActionResult> Edit(Guid id, EditPresetModel model)
    {
        var userId = _userAccessor.GetUserId();
        var result = await _mediator.Send(new EditPresetCommand(id, model, userId));
        return result.ToActionResult();
    }

    /// <summary>
    /// Удалить пресет.
    /// </summary>
    /// <param name="id"> Уникальный идентификатор. </param>
    /// <returns> Результат выполнения команды. </returns>
    /// <response code="204"> Успешно. </response>
    [HttpDelete("{id:Guid}")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> Remove(Guid id)
    {
        var userId = _userAccessor.GetUserId();
        var result = await _mediator.Send(new RemovePresetCommand(id, userId));
        return result.ToActionResult();
    }

    /// <summary>
    /// Применить пресет на устройство.
    /// </summary>
    /// <param name="id"> Идентификатор пресета. </param>
    /// <param name="deviceIds"> Идентификаторы устройств. </param>
    /// <returns> Результат выполнения запроса. </returns>
    /// <response code="200"> Успешно. </response>
    /// <response code="400">
    /// Майнинг устройство или пресет не найдены.
    /// </response>
    [HttpPost("{id:Guid}/apply")]
    [ProducesResponseType(typeof(Guid), 200)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> ApplyPreset(Guid id, params Guid[] deviceIds)
    {
        var userId = _userAccessor.GetUserId();
        var result = await _mediator.Send(
            new ApplyPresetCommand(userId, id, deviceIds));
        return result.ToActionResult();
    }
}
