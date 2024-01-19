using Kernel.UseCases;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MINUX.Backend.Worker.Core;
using MINUX.Backend.Worker.UseCases.Commands.AddWalletCommand;
using MINUX.Backend.Worker.UseCases.Queries.GetWalletsQuery;

namespace MINUX.Backend.Worker.Controllers;

[Route("api/wallet")]
[ApiController]
public class WalletController : ControllerBase
{
    private readonly IMediator _mediator;

    public WalletController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public IAsyncEnumerable<Wallet> GetAll()
    {
        return _mediator.CreateStream(new GetWalletsQuery());
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddWalletCommand request)
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