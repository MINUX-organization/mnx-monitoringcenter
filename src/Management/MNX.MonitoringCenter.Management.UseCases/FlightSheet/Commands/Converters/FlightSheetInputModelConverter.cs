using AutoMapper;
using MNX.MonitoringCenter.Management.Core.FlightSheet;
using MNX.MonitoringCenter.Management.UseCases.FlightSheets.Commands.Models;

namespace MNX.MonitoringCenter.Management.UseCases.FlightSheets.Commands.Converters;

/// <summary>
/// Конвертер входной модели полётного листа.
/// </summary>
internal class FlightSheetInputModelConverter : ITypeConverter<FlightSheetInputModel, FlightSheetBase>
{
    public FlightSheetBase Convert(FlightSheetInputModel source, FlightSheetBase destination, ResolutionContext context)
    {
        if (source is CpuFlightSheetInputModel cpuModel)
        {
            return context.Mapper.Map<CpuFlightSheet>(cpuModel);
        }
        else if (source is GpuFlightSheetInputModel gpuModel)
        {
            return context.Mapper.Map<CpuFlightSheet>(gpuModel);
        }

        return default!;
    }
}
