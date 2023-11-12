using MediatR;
using Microsoft.AspNetCore.Mvc;
using MINUX.Backend.Unit.Core;
using MINUX.Backend.Unit.UseCases.Queries.GetCryptocurrenciesQuery;

namespace MINUX.Backend.Unit.Controllers;

[Route("api/flightSheet")]
[ApiController]
public class FlightSheetController : ControllerBase
{
    private readonly IMediator _mediator;

    public FlightSheetController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public IAsyncEnumerable<FlightSheet> GetAll()
    {
        return _mediator.CreateStream(new GetFlightSheetsQuery());
    }

    [HttpPost]
    public Task<IActionResult> Create()
    {
        throw new NotImplementedException();
    }

    [HttpPost("{id:Guid}/apply")]
    public Task<IActionResult> Apply(Guid id)
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
