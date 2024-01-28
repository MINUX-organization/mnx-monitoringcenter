using MediatR;
using MINUX.Backend.Worker.Core.HardwareParameters.Harddrive;

namespace MINUX.Backend.Worker.UseCases.Queries.HardwareParameters.GetHarddriveData;

/// <summary>
/// Запрос на получение информации о жётских дисках
/// </summary>
public struct GetHarddrivesDataQuery : IStreamRequest<Harddrive> { }
