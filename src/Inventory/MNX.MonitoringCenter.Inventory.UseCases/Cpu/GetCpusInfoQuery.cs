using MediatR;

namespace MNX.MonitoringCenter.Inventory.UseCases.Cpu;

using Cpu = Contracts.Cpu.Cpu;

internal class GetCpusInfoQuery : IStreamRequest<Cpu>
{
}
