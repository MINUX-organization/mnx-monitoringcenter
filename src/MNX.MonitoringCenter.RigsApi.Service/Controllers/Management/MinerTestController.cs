using MediatR;
using Microsoft.AspNetCore.Mvc;
using MNX.MonitoringCenter.Management.Contracts.MinerContracts;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;
using MNX.MonitoringCenter.Management.UseCases.Mining.Miner;

namespace MNX.MonitoringCenter.RigsApi.Service.Controllers.Management
{
    [Route("api/[controller]")]
    [ApiController]
    public class MinerTestController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MinerTestController(IMediator mediator)
        {
            _mediator = mediator ?? throw new NotImplementedException(nameof(mediator));
        }

        [HttpPost]
        public IActionResult Index(MinerInputModel model)
        {
            var result = _mediator.Send(new TestMinerCommand(model));
            return Ok(result);
        }

        [HttpPatch]
        public IActionResult Test()
        {
            var result = MiningDeviceType.GPU;
            return Ok(result);
        }
    }
}
