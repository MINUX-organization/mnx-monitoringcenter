using MediatR;
using Microsoft.AspNetCore.Mvc;
using MINUX.Backend.Unit.Core;
using MINUX.Backend.Unit.UseCases.Queries.GetPoolsQuery;

namespace MINUX.Backend.Unit.Controllers;

[Route("api/pool")]
[ApiController]
public class PoolController : ControllerBase
{
    private readonly IMediator _mediator;

    public PoolController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public IAsyncEnumerable<Pool> GetAll()
    {
        return _mediator.CreateStream(new GetPoolsQuery());
    }

    [HttpPost]
    public Task<IActionResult> Create()
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{id:Guid}")]
    public Task<IActionResult> Delete(Guid id)
    {
        throw new NotImplementedException();
    }
}
