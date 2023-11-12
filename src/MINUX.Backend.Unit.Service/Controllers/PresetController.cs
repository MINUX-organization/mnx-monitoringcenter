using MediatR;
using Microsoft.AspNetCore.Mvc;
using MINUX.Backend.Unit.Core;
using MINUX.Backend.Unit.UseCases.Queries.GetPresetsQuery;

namespace MINUX.Backend.Unit.Controllers;

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
    public IAsyncEnumerable<Preset> GetAll()
    {
        return _mediator.CreateStream(new GetPresetsQuery());
    }

    [HttpPost]
    public Task<IActionResult> Create()
    {
        throw new NotImplementedException();
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
