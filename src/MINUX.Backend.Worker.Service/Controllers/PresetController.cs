using Kernel.UseCases;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MINUX.Backend.Worker.Core;
using MINUX.Backend.Worker.UseCases.Commands.SavePresetCommand;
using MINUX.Backend.Worker.UseCases.Queries.GetPresetsQuery;

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

    [HttpGet("{gpuName}")]
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
    public Task<IActionResult> Edit(Guid id)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{id:Guid}")]
    public Task<IActionResult> Delete(Guid id)
    {
        throw new NotImplementedException();
    }
}
