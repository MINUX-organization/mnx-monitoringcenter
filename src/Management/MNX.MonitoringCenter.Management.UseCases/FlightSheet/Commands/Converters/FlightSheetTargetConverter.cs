using AutoMapper;
using MNX.MonitoringCenter.Management.Contracts.FlightSheet.Target;
using MNX.MonitoringCenter.Management.Core.FlightSheet.Target;

namespace MNX.MonitoringCenter.Management.UseCases.FlightSheet.Commands.Converters;

/// <summary>
/// Конвертер таргета полётного листа.
/// </summary>
internal class FlightSheetTargetConverter : ITypeConverter<FlightSheetTargetBase, FlightSheetTargetModelBase>
{
    public FlightSheetTargetModelBase Convert(FlightSheetTargetBase source, FlightSheetTargetModelBase destination, ResolutionContext context)
    {
        if (source is CpuFlightSheetTarget cpuFlightSheet)
        {
            return context.Mapper.Map<CpuFlightSheetTargetModel>(cpuFlightSheet);
        }
        else if (source is GpuFlightSheetTarget gpuFlightSheet)
        {
            return context.Mapper.Map<GpuFlightSheetTargetModel>(gpuFlightSheet);
        }

        return default!;
    }
}
