using Kernel.UseCases;
using MediatR;
using MINUX.Backend.Worker.Core.HardwareParameters;

namespace MINUX.Backend.Worker.UseCases.Queries.HardwareParameters.GetSystemInfo;

/// <summary>
/// Запрос на получение информации о системе
/// </summary>
public class GetSystemInfoQuery : IRequest<Result<SystemInfo>> { }
