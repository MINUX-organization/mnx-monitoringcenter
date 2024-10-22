using AutoMapper;
using MNX.MonitoringCenter.Management.Core.FlightSheet.Target;
using MNX.MonitoringCenter.Management.UseCases.FlightSheet.Commands.Models.Target;

namespace MNX.MonitoringCenter.Management.UseCases.FlightSheet.Converters;

/// <summary>
/// Конвертер входной модели таргета полётного листа.
/// </summary>
internal class FlightSheetTargetInputModelConverter : ITypeConverter<FlightSheetTargetInputModel, FlightSheetTargetBase>
{
    public FlightSheetTargetBase Convert(FlightSheetTargetInputModel source, FlightSheetTargetBase destination, ResolutionContext context)
    {
        if (source is CpuFlightSheetTargetInputModel cpuModel)
        {
            return context.Mapper.Map<CpuFlightSheetTarget>(cpuModel);
        }
        else if (source is GpuFlightSheetTargetInputModel gpuModel)
        {
            return context.Mapper.Map<GpuFlightSheetTarget>(gpuModel);
        }

        return default!;
    }
}
