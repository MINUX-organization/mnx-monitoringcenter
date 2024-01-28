using MediatR;
using MINUX.Backend.Worker.Core.HardwareParameters.Gpu;

namespace MINUX.Backend.Worker.UseCases.Queries.HardwareParameters.GetGpuData;

/// <summary>
/// Запрос на получение праметров о видеокартах
/// </summary>
public struct GetGpusDataQuery : IStreamRequest<Gpu> { }
