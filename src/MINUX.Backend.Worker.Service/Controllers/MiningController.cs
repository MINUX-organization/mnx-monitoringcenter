using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MINUX.Backend.Worker.Controllers;

[Route("api/mining")]
[ApiController]
public class MiningController : ControllerBase
{
    private readonly IMediator _mediator;

    public MiningController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("run")]
    public Task<IActionResult> Run(Guid[] ids)
    {
        throw new NotImplementedException();
    }

    [HttpPost("stop")]
    public Task<IActionResult> Stop(Guid[] ids)
    {
        throw new NotImplementedException();
    }
}
