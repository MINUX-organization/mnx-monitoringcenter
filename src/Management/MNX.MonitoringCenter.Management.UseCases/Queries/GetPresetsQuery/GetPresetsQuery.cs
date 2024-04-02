using MediatR;
using MNX.MonitoringCenter.Management.Core;

namespace MNX.MonitoringCenter.Management.UseCases.Queries.GetPresetsQuery;

/// <summary>
/// Запрос на получение сохранённых пресетов для выбранной серии GPU
/// </summary>
public class GetPresetsQuery : IStreamRequest<Preset>
{
    /// <summary>
    /// Название GPU
    /// </summary>
    public string? GpuName { get; }

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public long UserId { get; set; }

    public GetPresetsQuery(string? gpuName, long userId)
    {
        GpuName = gpuName;
        UserId = userId;
    }
}