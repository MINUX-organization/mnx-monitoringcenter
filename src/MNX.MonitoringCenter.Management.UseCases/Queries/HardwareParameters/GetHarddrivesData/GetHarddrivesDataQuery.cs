using MediatR;
using MNX.MonitoringCenter.Management.Core.HardwareParameters.Harddrive;

namespace MNX.MonitoringCenter.Management.UseCases.Queries.HardwareParameters.GetHarddriveData;

/// <summary>
/// Запрос на получение информации о жётских дисках
/// </summary>
public struct GetHarddrivesDataQuery : IStreamRequest<Harddrive> { }
