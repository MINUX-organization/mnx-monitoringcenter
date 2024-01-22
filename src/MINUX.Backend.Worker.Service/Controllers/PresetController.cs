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

[Route("api/preset")]
[ApiController]
public class PresetController : ControllerBase
{
    private readonly IMediator _mediator;

    public PresetController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public IAsyncEnumerable<Preset> GetPresets(string? gpuName)
    {
        return _mediator.CreateStream(new GetPresetsQuery(gpuName));
    }

    [HttpPost]
    public async Task<IActionResult> Save(SavePresetCommand command)
    {
        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }

    [HttpPut("{id:Guid}")]
    public async Task<IActionResult> Update(Guid id, PresetModel model)
    {
        var result = await _mediator.Send(new UpdatePresetCommand(id, model));
        return result.ToActionResult();
    }

    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> Remove(Guid id)
    {
        var result = await _mediator.Send(new RemovePresetCommand(id));
        return result.ToActionResult();
    }
}
