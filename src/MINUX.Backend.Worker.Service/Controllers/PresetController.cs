using Kernel.UseCases;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MINUX.Backend.Worker.Core;
using MINUX.Backend.Worker.UseCases.Commands.Presets.SavePreset;
using MINUX.Backend.Worker.UseCases.Commands.Presets.RemovePreset;
using MINUX.Backend.Worker.UseCases.Queries.GetPresetsQuery;
using MINUX.Backend.Worker.UseCases.Commands.Presets;
using MINUX.Backend.Worker.UseCases.Commands.Presets.UpdatePreset;

namespace MINUX.Backend.Worker.Controllers;

/// <summary>
/// Предоставляет REST API для работы с пресетами
/// </summary>
[Route("api/preset")]
[ApiController]
public class PresetController : ControllerBase
{
    private readonly IMediator _mediator;

    public PresetController(IMediator mediator)
    {
        _mediator = mediator;
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
        return _mediator.CreateStream(new GetPresetsQuery(gpuName));
    }

    /// <summary>
    /// Сохранить пресет к указанной серии видеокарт
    /// </summary>
    /// <param name="command"> Команда с параметрами пресета </param>
    /// <returns> Результат выполнения команды </returns>
    /// <response code="201"> Успешно </response>
    /// <response code="400">
    /// Переданные параметры не прошли валидацию или не была найдена GPU с указанным названием
    /// </response>
    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(typeof(List<Preset>), 400)]
    public async Task<IActionResult> Save(SavePresetCommand command)
    {
        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }

    /// <summary>
    /// Обновить пресет
    /// </summary>
    /// <param name="id"> Уникальный идентификатор </param>
    /// <param name="model"> Модель пресета </param>
    /// <returns> Результат выполнения команды </returns>
    /// <response code="200"> Успешно </response>
    /// <response code="400">
    /// Переданные параметры не прошли валидацию или не был найден пресет с переданным id
    /// </response>
    [HttpPut("{id:Guid}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(List<Preset>), 400)]
    public async Task<IActionResult> Update(Guid id, PresetModel model)
    {
        var result = await _mediator.Send(new UpdatePresetCommand(id, model));
        return result.ToActionResult();
    }

    /// <summary>
    /// Удалить пресет
    /// </summary>
    /// <param name="id"> Уникальный идентификатор </param>
    /// <returns> Результат выполнения команды </returns>
    /// <response code="200"> Успешно </response>
    /// <response code="400"> Не был найден пресет с переданным идентификатором </response>
    [HttpDelete("{id:Guid}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(List<Preset>), 400)]
    public async Task<IActionResult> Remove(Guid id)
    {
        var result = await _mediator.Send(new RemovePresetCommand(id));
        return result.ToActionResult();
    }
}
