using MediatR;
using MINUX.Backend.Worker.Core.HardwareParameters.Cpu;

namespace MINUX.Backend.Worker.UseCases.Queries.HardwareParameters.GetCpusData;

/// <summary>
/// Запрос на получение параметров о CPUs
/// </summary>
public struct GetCpusDataQuery : IStreamRequest<Cpu> { }
