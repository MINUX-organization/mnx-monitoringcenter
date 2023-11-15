using MediatR;
using Microsoft.AspNetCore.Mvc;
using MINUX.Backend.Unit.Core;
using MINUX.Backend.Unit.UseCases.Commands.AddPoolCommand;
using MINUX.Backend.Unit.UseCases.Queries.GetPoolsQuery;
using Kernel.UseCases;

namespace MINUX.Backend.Unit.Controllers;

[Route("api/pool")]
[ApiController]
public class PoolController : ControllerBase
{
    private readonly IMediator _mediator;

    public PoolController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpGet]
    public IAsyncEnumerable<Pool> GetAll()
    {
        return _mediator.CreateStream(new GetPoolsQuery());
    }

    [HttpPost]
    public async Task<IActionResult> Create(AddPoolCommand request)
    {
        var result = await _mediator.Send(request);
        return result.ToActionResult();
    }

    [HttpDelete("{id:Guid}")]
    public Task<IActionResult> Delete(Guid id)
    {
        throw new NotImplementedException();
    }
}
