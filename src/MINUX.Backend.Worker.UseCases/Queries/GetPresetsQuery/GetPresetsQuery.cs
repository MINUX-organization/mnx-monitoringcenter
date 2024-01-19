using MediatR;
using MINUX.Backend.Worker.Core;

namespace MINUX.Backend.Worker.UseCases.Queries.GetPresetsQuery;

/// <summary>
/// Запрос на получение сохранённых пресетов для выбранной серии GPU
/// </summary>
public class GetPresetsQuery : IStreamRequest<Preset>
{
    /// <summary>
    /// Название GPU
    /// </summary>
    public string? GpuName { get; }

    public GetPresetsQuery(string? gpuName)
    {
        GpuName = gpuName;
    }
}