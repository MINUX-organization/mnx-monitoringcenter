using MediatR;
using Microsoft.AspNetCore.Mvc;
using MINUX.Backend.Unit.Core;
using MINUX.Backend.Unit.UseCases.Queries.GetCryptocurrenciesQuery;

namespace MINUX.Backend.Unit.Controllers;

[Route("api/cryptocurrency")]
[ApiController]
public class CryptocurrencyController : ControllerBase
{
    private readonly IMediator _mediator;

    public CryptocurrencyController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public IAsyncEnumerable<Cryptocurrency> GetAll()
    {
        return _mediator.CreateStream(new GetCryptocurrenciesQuery());
    }

    [HttpPost]
    public Task<IActionResult> Create(string shortName)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{shortName}")]
    public Task<IActionResult> Delete(string shortName)
    {
        throw new NotImplementedException();
    }
}
