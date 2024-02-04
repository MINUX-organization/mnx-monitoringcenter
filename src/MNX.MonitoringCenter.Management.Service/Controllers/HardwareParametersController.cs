using Kernel.UseCases;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MNX.MonitoringCenter.Management.Core.HardwareParameters;
using MNX.MonitoringCenter.Management.Core.HardwareParameters.Cpu;
using MNX.MonitoringCenter.Management.Core.HardwareParameters.Gpu;
using MNX.MonitoringCenter.Management.Core.HardwareParameters.Harddrive;
using MNX.MonitoringCenter.Management.UseCases.Queries.HardwareParameters.GetCpusData;
using MNX.MonitoringCenter.Management.UseCases.Queries.HardwareParameters.GetGpuData;
using MNX.MonitoringCenter.Management.UseCases.Queries.HardwareParameters.GetHarddriveData;
using MNX.MonitoringCenter.Management.UseCases.Queries.HardwareParameters.GetMotherboardData;
using MNX.MonitoringCenter.Management.UseCases.Queries.HardwareParameters.GetRamsData;
using MNX.MonitoringCenter.Management.UseCases.Queries.HardwareParameters.GetSystemInfo;

namespace MNX.MonitoringCenter.Management.Controllers;

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
