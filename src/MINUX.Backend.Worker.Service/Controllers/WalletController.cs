using Kernel.UseCases;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MINUX.Backend.Worker.Core;
using MINUX.Backend.Worker.UseCases.Commands.Wallets;
using MINUX.Backend.Worker.UseCases.Commands.Wallets.AddWallet;
using MINUX.Backend.Worker.UseCases.Commands.Wallets.EditWallet;
using MINUX.Backend.Worker.UseCases.Commands.Wallets.RemoveWallet;
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
    public async Task<IActionResult> Add(WalletModel model)
    {
        var result = await _mediator.Send(new AddWalletCommand(model));
        return result.ToActionResult();
    }

    [HttpPut("{id:Guid}")]
    public async Task<IActionResult> Edit(Guid id, WalletModel model)
    {
        var result = await _mediator.Send(new EditWalletCommand(id, model));
        return result.ToActionResult();
    }

    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new RemoveWalletCommand(id));
        return result.ToActionResult();
    }
}