using Kernel.UseCases;
using MediatR;
using MINUX.Backend.Worker.Core.HardwareParameters.Motherboard;

namespace MINUX.Backend.Worker.UseCases.Queries.HardwareParameters.GetMotherboardData;

/// <summary>
/// Запрос на получение информации о материнской плате
/// </summary>
public struct GetMotherboardDataQuery : IRequest<Result<Motherboard>> { }
