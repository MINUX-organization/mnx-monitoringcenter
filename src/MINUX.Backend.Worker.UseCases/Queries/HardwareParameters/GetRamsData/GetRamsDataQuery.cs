using MediatR;
using MINUX.Backend.Worker.Core.HardwareParameters;

namespace MINUX.Backend.Worker.UseCases.Queries.HardwareParameters.GetRamsData;

/// <summary>
/// Запрос на получение информации о плашках оперативной памяти
/// </summary>
public struct GetRamsDataQuery : IStreamRequest<Ram> { }
