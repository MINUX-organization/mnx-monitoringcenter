using AutoMapper;
using MNX.MonitoringCenter.Management.Contracts.FlightSheet;
using MNX.MonitoringCenter.Management.Core.FlightSheet;

namespace MNX.MonitoringCenter.Management.UseCases.FlightSheets.Commands.Converters;

/// <summary>
/// Конвертер полётного листа.
/// </summary>
internal class FlightSheetConverter : ITypeConverter<FlightSheetBase, FlightSheetModelBase>
{
    public FlightSheetModelBase Convert(FlightSheetBase source, FlightSheetModelBase destination, ResolutionContext context)
    {
        if (source is CpuFlightSheet cpuFlightSheet)
        {
            return context.Mapper.Map<CpuFlightSheetModel>(cpuFlightSheet);
        }
        else if (source is GpuFlightSheet gpuFlightSheet)
        {
            return context.Mapper.Map<GpuFlightSheetModel>(gpuFlightSheet);
        }

        return default!;
    }
}
