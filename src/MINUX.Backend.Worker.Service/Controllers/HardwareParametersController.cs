using Kernel.UseCases;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MINUX.Backend.Worker.Core.HardwareParameters;
using MINUX.Backend.Worker.Core.HardwareParameters.Cpu;
using MINUX.Backend.Worker.Core.HardwareParameters.Gpu;
using MINUX.Backend.Worker.Core.HardwareParameters.Harddrive;
using MINUX.Backend.Worker.UseCases.Queries.HardwareParameters.GetCpusData;
using MINUX.Backend.Worker.UseCases.Queries.HardwareParameters.GetGpuData;
using MINUX.Backend.Worker.UseCases.Queries.HardwareParameters.GetHarddriveData;
using MINUX.Backend.Worker.UseCases.Queries.HardwareParameters.GetMotherboardData;
using MINUX.Backend.Worker.UseCases.Queries.HardwareParameters.GetRamsData;
using MINUX.Backend.Worker.UseCases.Queries.HardwareParameters.GetSystemInfo;

namespace MINUX.Backend.Worker.Controllers;

/// <summary>
/// Контроллер, предоставляющий Rest API для получения параметров железа и системы
/// </summary>
[Route("api/hardwareParameters")]
[ApiController]
public class HardwareParametersController : ControllerBase
{
    private readonly IMediator _mediator;

    public HardwareParametersController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpGet("cpu")]
    public IAsyncEnumerable<Cpu> GetCpuStaticData()
    {
        return _mediator.CreateStream(new GetCpusDataQuery());
    }

    [HttpGet("gpu")]
    public IAsyncEnumerable<Gpu> GetGpusStaticData()
    {
        return _mediator.CreateStream(new GetGpusDataQuery());
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
