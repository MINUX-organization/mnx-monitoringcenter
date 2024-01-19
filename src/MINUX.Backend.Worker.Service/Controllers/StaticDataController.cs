using Kernel.UseCases;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MINUX.Backend.Worker.Core.StaticData;
using MINUX.Backend.Worker.Core.StaticData.Gpu;
using MINUX.Backend.Worker.Core.StaticData.Harddrive;
using MINUX.Backend.Worker.UseCases.Queries.GetCpuQuery;
using MINUX.Backend.Worker.UseCases.Queries.GetGpuDataQuery;
using MINUX.Backend.Worker.UseCases.Queries.GetHarddriveDataQuery;
using MINUX.Backend.Worker.UseCases.Queries.GetMotherboardDataQuery;
using MINUX.Backend.Worker.UseCases.Queries.GetRamDataQuery;
using MINUX.Backend.Worker.UseCases.Queries.GetStaticDataQuery;
using MINUX.Backend.Worker.UseCases.Queries.GetSystemInfoQuery;

namespace MINUX.Backend.Worker.Controllers;

[Route("api/staticData")]
[ApiController]
public class StaticDataController : ControllerBase
{

    private readonly IMediator _mediator;

    public StaticDataController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetFullStaticData()
    {
        var result = await _mediator.Send(new GetStaticDataQuery());
        return result.ToActionResult();
    }

    [HttpGet("gpu")]
    public IAsyncEnumerable<Gpu> GetGpusStaticData()
    {
        return _mediator.CreateStream(new GetGpusDataQuery());
    }

    [HttpGet("cpu")]
    public async Task<IActionResult> GetCpuStaticData()
    {
        var result = await _mediator.Send(new GetCpuQuery());
        return result.ToActionResult();
    }

    [HttpGet("harddrive")]
    public IAsyncEnumerable<Harddrive> GetHarddrivesStaticData()
    {
        return _mediator.CreateStream(new GetHarddrivesDataQuery());
    }

    [HttpGet("ram")]
    public IAsyncEnumerable<Ram> GetRamsStaticData()
    {
        return _mediator.CreateStream(new GetRamsDataQuery());
    }

    [HttpGet("motherboard")]
    public async Task<IActionResult> GetMotherboardStaticData()
    {
        var result = await _mediator.Send(new GetMotherboardDataQuery());
        return result.ToActionResult();
    }

    [HttpGet("system")]
    public async Task<IActionResult> GetSystemStaticData()
    {
        var result = await _mediator.Send(new GetSystemInfoQuery());
        return result.ToActionResult();
    }
}
