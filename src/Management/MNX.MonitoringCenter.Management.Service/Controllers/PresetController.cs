using Kernel.UseCases;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets.RemovePreset;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets.SavePreset;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets.UpdatePreset;
using MNX.MonitoringCenter.Management.UseCases.Queries.GetPresetsQuery;

namespace MNX.MonitoringCenter.Management.Controllers;

/// <summary>
/// Предоставляет REST API для работы с пресетами
/// </summary>
[Route("api/presets")]
[ApiController]
public class PresetController : ControllerBase
{
    private readonly IMediator _mediator;

    public PresetController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
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
    [ProducesResponseType(typeof(IAsyncEnumerable<Preset>), 200)]
    public IAsyncEnumerable<Preset> GetPresets(string? gpuName)
    {
        var userId = 1;
        return _mediator.CreateStream(new GetPresetsQuery(gpuName, userId));
    }

    /// <summary>
    /// Сохранить пресет к указанной серии видеокарт
    /// </summary>
    /// <param name="model"> Входная модель пресета </param>
    /// <returns> Результат выполнения команды </returns>
    /// <response code="201"> Успешно </response>
    /// <response code="400">
    /// Переданные параметры не прошли валидацию или не была найдена GPU с указанным названием
    /// </response>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), 201)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> Save(SavePresetInputModel model)
    {
        var userId = 1;
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
    public async Task<IActionResult> Update(Guid id, PresetInputModel model)
    {
        var userId = 1;
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
        var userId = 1;
        var result = await _mediator.Send(new RemovePresetCommand(id, userId));
        return result.ToActionResult();
    }
}
